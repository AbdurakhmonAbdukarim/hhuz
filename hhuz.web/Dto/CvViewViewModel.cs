using hhuz.Models;

namespace hhuz.Dto;

public class CvViewViewModel
{
    public Cvs Cv { get; set; } = null!;
    public Positions Position { get; set; } = null!;
    public List<Projects> Projects { get; set; } = new();
    public List<CandidateAttributeValues> AttributeValues { get; set; } = new();  // ← QOSHISH
    public bool IsOwner { get; set; }
    public bool IsRecruiter { get; set; }
    public int LikeCount { get; set; }
    public bool HasLiked { get; set; }
}