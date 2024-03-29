using FluentValidation;

namespace AlbaconTest.Services.Models;

public class Document
{
    /// <summary>
    /// Guid zvolen protože pro práci s různými datovými úložišti je vhodnější než int
    /// </summary>
    public Guid Identifier { get; set; }

    public string[] Tags { get; set; }

    public string Data { get; set; }


}

public class DocumentValidator : AbstractValidator<Document>
{
    public DocumentValidator()
    {
        RuleFor(x => x.Tags).NotEmpty();
        RuleFor(x => x.Data).NotEmpty();
    }
}
