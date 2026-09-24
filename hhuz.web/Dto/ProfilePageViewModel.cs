using hhuz.Models;

namespace hhuz.Dto;

public class ProfilePageViewModel
{
    public Profiles? Profile { get; set; }
    public List<CandidateAttributeValues> AttributeValues { get; set; } = new();
    public List<Projects> Projects { get; set; } = new();
    public List<Cvs> Cvs { get; set; } = new();
    public List<Attributes> AvailableAttributes { get; set; } = new();
}