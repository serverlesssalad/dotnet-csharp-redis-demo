using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Text.Json;

[Route("api/[controller]")]
[ApiController]
public class WordsController : ControllerBase
{
    private readonly IConnectionMultiplexer _redisConnection;

    public WordsController(IConnectionMultiplexer redisConnection)
    {
        _redisConnection = redisConnection;
    }

    private IDatabase Redis => _redisConnection.GetDatabase();

    // GET: api/words
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WordModel>>> GetAllWords()
    {
        var keys = await Redis.SetMembersAsync("words");
        var words = keys.Select(key => new WordModel { Word = key.ToString() }).ToList();
        return Ok(words);
    }

    // GET: api/words/{word_id}
    [HttpGet("{word}")]
    public async Task<ActionResult<WordModel>> GetWord(string word)
    {
        var exists = await Redis.SetContainsAsync("words", word);
        if (!exists)
        {
            return NotFound(new { message = "Word not found" });
        }

        return Ok(new WordModel { Word = word });
    }

    // POST: api/words
    [HttpPost]
    public async Task<ActionResult<WordModel>> CreateWord([FromBody] WordModel word)
    {
        if (string.IsNullOrEmpty(word.Word))
        {
            return BadRequest(new { message = "Invalid word data" });
        }

        await Redis.SetAddAsync("words", word.Word);
        return CreatedAtAction(nameof(GetWord), new { word = word.Word }, word);
    }

    // PUT: api/words/{word_id}
    [HttpPut("{word}")]
    public async Task<ActionResult<WordModel>> UpdateWord(string word, [FromBody] WordUpdateModel wordUpdate)
    {
        if (string.IsNullOrEmpty(wordUpdate.Word))
        {
            return BadRequest(new { message = "No fields to update" });
        }

        var exists = await Redis.SetContainsAsync("words", word);
        if (!exists)
        {
            return NotFound(new { message = "Word not found" });
        }

        await Redis.SetRemoveAsync("words", word);
        await Redis.SetAddAsync("words", wordUpdate.Word);
        return Ok(new WordModel { Word = wordUpdate.Word });
    }

    // DELETE: api/words/{word_id}
    [HttpDelete("{word}")]
    public async Task<ActionResult> DeleteWord(string word)
    {
        var exists = await Redis.SetContainsAsync("words", word);
        if (!exists)
        {
            return NotFound(new { message = "Word not found" });
        }

        await Redis.SetRemoveAsync("words", word);
        return NoContent();
    }

    // GET: /hello
    [HttpGet("hello")]
    public async Task<ActionResult<WordModel>> GetHelloWorld()
    {
        var words = await Redis.SetMembersAsync("words");
        var word = words.FirstOrDefault()?.ToString() ?? "None";
        return Ok(new WordModel { Word = $"Hello {word}" });
    }

    // GET: /health
    [HttpGet("health")]
    public ActionResult HealthCheck()
    {
        try
        {
            if (_redisConnection.IsConnected)
            {
                return Ok(new { status = "healthy" });
            }
            return StatusCode(500, new { status = "unhealthy" });
        }
        catch
        {
            return StatusCode(500, new { status = "unhealthy" });
        }
    }
}
