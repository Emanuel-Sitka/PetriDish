using Godot;
using PetriDish.Genome;

namespace PetriDish.BioMorphs;

public partial class Biomorph : Node3D
{
    public BiomorphGenome Genome { get; set; }
    
    [Export]
    public PackedScene SegmentScene { get; set; }

    public override void _Ready()
    {
        Genome = new BiomorphGenome()
        {
            SpineSegmentGenomes = new List<SegmentGenome>()
            {
                new SegmentGenome(4f, 2f, 2f, 1.5f),
                new SegmentGenome(6f, 1.5f, 1.5f, 1.3f),
            }
        };

        var totalLength = Genome.TotalLength;

        var frontEndOfRemainingSpine = totalLength / 2;
        
        

        foreach (var segmentGenome in Genome.SpineSegmentGenomes)
        {
            var segment = (Segment)SegmentScene.Instantiate();
            segment.Configure(segmentGenome);

            var currentAttachmentPoint =
                Transform3D.Identity.Translated(new Vector3(0f, 0f, frontEndOfRemainingSpine - segment.Length / 2));
            
            segment.Transform = currentAttachmentPoint;
            AddChild(segment);
            
            frontEndOfRemainingSpine -= segment.Length;
        }
    }
}