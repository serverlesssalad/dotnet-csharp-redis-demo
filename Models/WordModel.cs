using System.ComponentModel.DataAnnotations;

public class WordModel
{
    [Required]
    public string? Word { get; set; }
}

public class WordUpdateModel
{
    public string Word { get; set; }
}
