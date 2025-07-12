namespace PetriDish.Genome;

public class SegmentGenome
{
    private float _midBulge = 1.001f;
    public float Length { get; set; } = 2.0f; 
    public float StartRadius { get; set; } = 0.5f; 
    public float EndRadius { get; set; } = 0.5f;

    public float MidBulge
    {
        get => _midBulge; 
        set => _midBulge = MathF.Max(value, 1.001f);
    }

    public SegmentGenome(float length, float startRadius, float endRadius, float midBulge)
    {
        Length = length;
        StartRadius = startRadius;
        EndRadius = endRadius;
        MidBulge = midBulge;
    }

    public SegmentGenome()
    {
    }
}