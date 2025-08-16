namespace PetriDish.Genome;

public class BiomorphGenome
{
    public List<SegmentGenome> SpineSegmentGenomes = new List<SegmentGenome>();
    
    public float TotalLength => SpineSegmentGenomes.Sum(segment => segment.Length);
}