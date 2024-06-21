namespace collabzone;

public record class UpdateProjectDTO
{
    public string? Name { get; init; }
    public bool Invite_guid { get; init; }
}
