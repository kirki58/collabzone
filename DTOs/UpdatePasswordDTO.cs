namespace collabzone.DTOS;

public record class UpdatePasswordDTO
{
    public required string Old_Password { get; init; }
    public required string New_Password { get; init; }     
}
