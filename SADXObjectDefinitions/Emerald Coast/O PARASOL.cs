﻿using System;
using System.Collections.Generic;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using SonicRetro.SAModel;
using SonicRetro.SAModel.Direct3D;
using SonicRetro.SAModel.SAEditorCommon.DataTypes;
using SonicRetro.SAModel.SAEditorCommon.SETEditing;

namespace SADXObjectDefinitions.EmeraldCoast
{
	public abstract class OParasol : ObjectDefinition
	{
		protected NJS_OBJECT model1;
		protected Mesh[] meshes1;
		protected NJS_OBJECT model2;
		protected Mesh[] meshes2;
		protected NJS_OBJECT model3;
		protected Mesh[] meshes3;
		protected NJS_OBJECT model4;
		protected Mesh[] meshes4;
		protected NJS_OBJECT model5;
		protected Mesh[] meshes5;

		public override HitResult CheckHit(SETItem item, Vector3 Near, Vector3 Far, Viewport Viewport, Matrix Projection, Matrix View, MatrixStack transform)
		{
			if (item.Scale.X == 0 || item.Scale.X == 5 || item.Scale.X == 10 || item.Scale.X == -5)
			{
				transform.Push();
				transform.NJTranslate(item.Position);
				HitResult result = model1.CheckHit(Near, Far, Viewport, Projection, View, transform, meshes1);
				transform.Pop();
				return result;
			}
			else if (item.Scale.X == 1 || item.Scale.X == 6 || item.Scale.X == 11 || item.Scale.X == -1)
			{
				transform.Push();
				transform.NJTranslate(item.Position);
				HitResult result = model2.CheckHit(Near, Far, Viewport, Projection, View, transform, meshes2);
				transform.Pop();
				return result;
			}
			else if (item.Scale.X == 2 || item.Scale.X == 7 || item.Scale.X == 12 || item.Scale.X == -2)
			{
				transform.Push();
				transform.NJTranslate(item.Position);
				HitResult result = model3.CheckHit(Near, Far, Viewport, Projection, View, transform, meshes3);
				transform.Pop();
				return result;
			}
			else if (item.Scale.X == 3 || item.Scale.X == 8 || item.Scale.X == 13 || item.Scale.X == -3)
			{
				transform.Push();
				transform.NJTranslate(item.Position);
				HitResult result = model4.CheckHit(Near, Far, Viewport, Projection, View, transform, meshes4);
				transform.Pop();
				return result;
			}
			else if (item.Scale.X == 4 || item.Scale.X == 9 || item.Scale.X == 14 || item.Scale.X == -4)
			{
				transform.Push();
				transform.NJTranslate(item.Position);
				HitResult result = model5.CheckHit(Near, Far, Viewport, Projection, View, transform, meshes5);
				transform.Pop();
				return result;
			}
			else
			{
				transform.Push();
				transform.NJTranslate(item.Position);
				HitResult result = model1.CheckHit(Near, Far, Viewport, Projection, View, transform, meshes1);
				transform.Pop();
				return result;
			}
		}

		public override List<RenderInfo> Render(SETItem item, Device dev, EditorCamera camera, MatrixStack transform)
		{
			if (item.Scale.X == 0 || item.Scale.X == 5 || item.Scale.X == 10 || item.Scale.X == -5)
			{
				List<RenderInfo> result = new List<RenderInfo>();
				transform.Push();
				transform.NJTranslate(item.Position);
				transform.NJRotateObject(item.Rotation);
				result.AddRange(model1.DrawModelTree(dev, transform, ObjectHelper.GetTextures("OBJ_BEACH"), meshes1));
				if (item.Selected)
					result.AddRange(model1.DrawModelTreeInvert(transform, meshes1));
				transform.Pop();
				return result;
			}
			else if (item.Scale.X == 1 || item.Scale.X == 6 || item.Scale.X == 11 || item.Scale.X == -1)
			{
				List<RenderInfo> result = new List<RenderInfo>();
				transform.Push();
				transform.NJTranslate(item.Position);
				transform.NJRotateObject(item.Rotation);
				result.AddRange(model2.DrawModelTree(dev, transform, ObjectHelper.GetTextures("OBJ_BEACH"), meshes2));
				if (item.Selected)
					result.AddRange(model2.DrawModelTreeInvert(transform, meshes2));
				transform.Pop();
				return result;
			}
			else if (item.Scale.X == 2 || item.Scale.X == 7 || item.Scale.X == 12 || item.Scale.X == -2)
			{
				List<RenderInfo> result = new List<RenderInfo>();
				transform.Push();
				transform.NJTranslate(item.Position);
				transform.NJRotateObject(item.Rotation);
				result.AddRange(model3.DrawModelTree(dev, transform, ObjectHelper.GetTextures("OBJ_BEACH"), meshes3));
				if (item.Selected)
					result.AddRange(model3.DrawModelTreeInvert(transform, meshes3));
				transform.Pop();
				return result;
			}
			else if (item.Scale.X == 3 || item.Scale.X == 8 || item.Scale.X == 13 || item.Scale.X == -3)
			{
				List<RenderInfo> result = new List<RenderInfo>();
				transform.Push();
				transform.NJTranslate(item.Position);
				transform.NJRotateObject(item.Rotation);
				result.AddRange(model4.DrawModelTree(dev, transform, ObjectHelper.GetTextures("OBJ_BEACH"), meshes4));
				if (item.Selected)
					result.AddRange(model4.DrawModelTreeInvert(transform, meshes4));
				transform.Pop();
				return result;
			}
			else if (item.Scale.X == 4 || item.Scale.X == 9 || item.Scale.X == 14 || item.Scale.X == -4)
			{
				List<RenderInfo> result = new List<RenderInfo>();
				transform.Push();
				transform.NJTranslate(item.Position);
				transform.NJRotateObject(item.Rotation);
				result.AddRange(model5.DrawModelTree(dev, transform, ObjectHelper.GetTextures("OBJ_BEACH"), meshes5));
				if (item.Selected)
					result.AddRange(model5.DrawModelTreeInvert(transform, meshes5));
				transform.Pop();
				return result;
			}
			else
			{
				List<RenderInfo> result = new List<RenderInfo>();
				transform.Push();
				transform.NJTranslate(item.Position);
				transform.NJRotateObject(item.Rotation);
				result.AddRange(model1.DrawModelTree(dev, transform, ObjectHelper.GetTextures("OBJ_BEACH"), meshes1));
				if (item.Selected)
					result.AddRange(model1.DrawModelTreeInvert(transform, meshes1));
				transform.Pop();
				return result;
			}
		}

		public override BoundingSphere GetBounds(SETItem item)
		{
			if (item.Scale.X == 0 || item.Scale.X == 5 || item.Scale.X == 10 || item.Scale.X == -5)
			{
				MatrixStack transform = new MatrixStack();
				transform.NJTranslate(item.Position);
				transform.NJRotateObject(item.Rotation);
				return ObjectHelper.GetModelBounds(model1, transform);
			}
			else if (item.Scale.X == 1 || item.Scale.X == 6 || item.Scale.X == 11 || item.Scale.X == -1)
			{
				MatrixStack transform = new MatrixStack();
				transform.NJTranslate(item.Position);
				transform.NJRotateObject(item.Rotation);
				return ObjectHelper.GetModelBounds(model2, transform);
			}
			else if (item.Scale.X == 2 || item.Scale.X == 7 || item.Scale.X == 12 || item.Scale.X == -2)
			{
				MatrixStack transform = new MatrixStack();
				transform.NJTranslate(item.Position);
				transform.NJRotateObject(item.Rotation);
				return ObjectHelper.GetModelBounds(model3, transform);
			}
			else if (item.Scale.X == 3 || item.Scale.X == 8 || item.Scale.X == 13 || item.Scale.X == -3)
			{
				MatrixStack transform = new MatrixStack();
				transform.NJTranslate(item.Position);
				transform.NJRotateObject(item.Rotation);
				return ObjectHelper.GetModelBounds(model4, transform);
			}
			else if (item.Scale.X == 4 || item.Scale.X == 9 || item.Scale.X == 14 || item.Scale.X == -4)
			{
				MatrixStack transform = new MatrixStack();
				transform.NJTranslate(item.Position);
				transform.NJRotateObject(item.Rotation);
				return ObjectHelper.GetModelBounds(model5, transform);
			}
			else
			{
				MatrixStack transform = new MatrixStack();
				transform.NJTranslate(item.Position);
				transform.NJRotateObject(item.Rotation);
				return ObjectHelper.GetModelBounds(model1, transform);
			}
		}

		internal int[] itemType = { 0, 1, 2, 3, 4 };

		private PropertySpec[] customProperties = new PropertySpec[] {
			new PropertySpec("Variant", typeof(Item), "Extended", null, null, (o) => (ParasolVariants)Math.Min(Math.Max((int)o.Scale.X, 0), 8), (o, v) => o.Scale.X = (int)v)
		};

		public override PropertySpec[] CustomProperties { get { return customProperties; } }

		public override float DefaultXScale { get { return 0; } }

		public override float DefaultYScale { get { return 0; } }

		public override float DefaultZScale { get { return 0; } }
	}

	public class Parasol : OParasol
	{
		public override void Init(ObjectData data, string name, Device dev)
		{
			model1 = ObjectHelper.LoadModel("Objects/Levels/Emerald Coast/O PARASOL_A.sa1mdl");
			meshes1 = ObjectHelper.GetMeshes(model1, dev);
			model2 = ObjectHelper.LoadModel("Objects/Levels/Emerald Coast/O PARASOL_B.sa1mdl");
			meshes2 = ObjectHelper.GetMeshes(model2, dev);
			model3 = ObjectHelper.LoadModel("Objects/Levels/Emerald Coast/O PARASOL_C.sa1mdl");
			meshes3 = ObjectHelper.GetMeshes(model3, dev);
			model4 = ObjectHelper.LoadModel("Objects/Levels/Emerald Coast/O PARASOL_D.sa1mdl");
			meshes4 = ObjectHelper.GetMeshes(model4, dev);
			model5 = ObjectHelper.LoadModel("Objects/Levels/Emerald Coast/O PARASOL_E.sa1mdl");
			meshes5 = ObjectHelper.GetMeshes(model5, dev);
		}

		public override string Name { get { return "Breakable Parasol, Chair, or Table"; } }
	}

	public enum ParasolVariants
	{
		Parasol_A,
		Parasol_B,
		Chair_A,
		Chair_B,
		Table
	}
}