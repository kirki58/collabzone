namespace collabzone.DTOS;

public record class CreateTaskDTO(int given_by, int given_at, DateTime due_at, string header, string description, int given_to);
