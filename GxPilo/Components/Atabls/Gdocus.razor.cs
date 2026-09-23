using GxShared.GxDtos;
using GxShared.Helpers;
using GxShared.Interfaces;

using GxPilo.Components.Uifrags;

namespace GxPilo.Components.Atabls
{
	//public partial class Gdocus : CompUICrudBase
	//{
	//	public Gdocus(IPendingChangesGuard guard) : base(guard)
	//	{
	//	}

	//	protected override void SubscribeToGuard()
	//	{
	//	}

	//	protected override string GetEntitySetName(EntityLevel level)
	//	{
	//		return base.GetEntitySetName(level);
	//	}
	//	protected override void OnEntitySaved(EntityLevel level, object entity)
	//	{
	//		switch (level)
	//		{
	//			case EntityLevel.Gtb:
	//				{
	//					var src = (GstablDto)entity;
	//					var target = MyDaGtbs.FirstOrDefault(x => x.Rowguid == src.Rowguid);
	//					if (target != null) CommitGtbDraft(target, src);
	//					break;
	//				}
	//			//case EntityLevel.Gt2:
	//			//	{
	//			//		var src = (GstablDto)entity;
	//			//		var target = MyDaGt2s.FirstOrDefault(x => x.Rowguid == src.Rowguid);
	//			//		if (target != null) CommitGt2Draft(target, src);
	//			//		break;
	//			//	}
	//		}
	//	}
	//	protected override void ClearAddRow(EntityLevel level, Guid rowguid)
	//	{
	//		switch (level)
	//		{
	//			case EntityLevel.Gtb:
	//				draftGtb = null;
	//				GtbRenderKey = Guid.Empty;
	//				break;

	//			//case EntityLevel.Gt2:
	//			//	draftGt2 = null;
	//			//	Gt2RenderKey = Guid.Empty;
	//			//	break;
	//		}

	//		//await InvokeAsync(StateHasChanged);
	//	}
	//	protected override void ClearEditRow(EntityLevel level, Guid rowguid)
	//	{
	//		switch (level)
	//		{
	//			case EntityLevel.Gtb:
	//				var idxp = GtbItems.FindIndex(x => x.Rowguid == rowguid);
	//				if (idxp >= 0)
	//					GtbItems[idxp] = DeepClone(_edGtbOriginals[rowguid]);
	//				draftGtb = null;
	//				GtbRenderKey = Guid.Empty;
	//				Console.WriteLine($"Cancel row {rowguid}: draftGtb={(draftGtb == null ? "null" : "set")}, GtbRenderKey={GtbRenderKey}");

	//				break;
	//			//case EntityLevel.Gt2:
	//			//	var idxr = Gt2Items.FindIndex(x => x.Rowguid == rowguid);
	//			//	if (idxr >= 0)
	//			//		Gt2Items[idxr] = DeepClone(_edGt2Originals[rowguid]);
	//			//	draftGt2 = null;
	//			//	Gt2RenderKey = Guid.Empty;
	//			//	break;
	//		}
	//	}
	//}
}
