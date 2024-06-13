namespace collabzone.DTOS;

public record class UpdateUserDTO
{
    public string? Name { get; init; }
    public string? Email { get; init; }
    public string? Password_hash { get; init; }     
}
