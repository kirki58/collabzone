namespace collabzone.Settings;

public record class EmailSettings{
    public required string SmtpServer { get; init; } 
    public required int SmtpPort { get; init; } 
    public required string SmtpUsername { get; init; } 
    public required string SmtpPassword { get; init; }
    public required string SenderEmail { get; init; }

    public EmailSettings()
    {
    }
};
