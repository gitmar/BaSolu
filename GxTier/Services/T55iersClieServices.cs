using GxShared.Sess;

namespace GxTie.Services
{
    //public class TiersClientService
    //{
    //    private readonly DxsoluContext _context;
    //    private readonly MovementService _movement;

    //    public TiersClientService(DxsoluContext context, MovementService movement)
    //    {
    //        _context = context;
    //        _movement = movement;
    //    }

    //    public async Task MoveTiersAsync(int tierspId, int newHie, int newPost)
    //    {
    //        var tiers = await _context.Tiersps.FindAsync(tierspId);
    //        if (tiers == null) throw new Exception("Tiers not found");

    //        var oldHie = tiers.Ihie;
    //        var oldPost = tiers.Ipst;

    //        // Update state
    //        tiers.Ihie = newHie;
    //        tiers.Ipst = newPost;

    //        await _context.SaveChangesAsync();

    //        // Log movement
    //        await _movement.LogPersonAsync(
    //            orgId: tiers.Idorg,
    //            tierspId: tiers.Id,
    //            fromHie: oldHie,
    //            toHie: newHie,
    //            fromPost: oldPost,
    //            toPost: newPost,
    //            type: MoveType.PersonMove,
    //            userId: "system"
    //        );
    //    }
    //}
}
