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
	public abstract class O_AOSummon : ObjectDefinition
	{
		protected NJS_OBJECT whale;
		protected Mesh[] whalemsh;
		protected NJS_OBJECT sphere;
		protected Mesh[] spheremsh;

		public override HitResult CheckHit(SETItem item, Vector3 Near, Vector3 Far, Viewport Viewport, Matrix Projection, Matrix View, MatrixStack transform)
		{
			HitResult result = HitResult.NoHit;
			transform.Push();
			transform.NJTranslate(item.Position);
			transform.NJRotateY(item.Rotation.Y);
			transform.NJScale(4.5f, 4.5f, 4.5f);
			transform.Push();
			result = HitResult.Min(result, sphere.CheckHit(Near, Far, Viewport, Projection, View, transform, spheremsh));
			transform.Pop();
			transform.Push();
			transform.NJTranslate(item.Position + item.Scale);
			transform.NJRotateY(item.Rotation.Y);
			transform.NJScale(0.40000001f, 0.40000001f, 0.40000001f);
			transform.Push();
			result = HitResult.Min(result, whale.CheckHit(Near, Far, Viewport, Projection, View, transform, whalemsh));
			transform.Pop();
			return result;
		}

		public override List<RenderInfo> Render(SETItem item, Device dev, EditorCamera camera, MatrixStack transform)
		{
			List<RenderInfo> result = new List<RenderInfo>();
			transform.Push();
			transform.NJTranslate(item.Position);
			transform.NJScale(4.5f, 4.5f, 4.5f);
			result.AddRange(sphere.DrawModelTree(dev, transform, null, spheremsh));
			if (item.Selected)
				result.AddRange(sphere.DrawModelTreeInvert(transform, spheremsh));
			transform.Pop();
			transform.Push();
			transform.NJTranslate(item.Position + item.Scale);
			transform.NJRotateY(item.Rotation.Y);
			transform.NJScale(0.40000001f, 0.40000001f, 0.40000001f);
			result.AddRange(whale.DrawModelTree(dev, transform, ObjectHelper.GetTextures("OBJ_BEACH"), whalemsh));
			if (item.Selected)
				result.AddRange(whale.DrawModelTreeInvert(transform, whalemsh));
			transform.Pop();
			return result;
		}

		public override BoundingSphere GetBounds(SETItem item)
		{
			MatrixStack transform = new MatrixStack();
			transform.NJTranslate(item.Position);
			transform.NJScale(4.5f, 4.5f, 4.5f);
			return ObjectHelper.GetModelBounds(sphere, transform);
		}
	}

	public class AOSummon : O_AOSummon
	{
		public override void Init(ObjectData data, string name, Device dev)
		{
			whale = ObjectHelper.LoadModel("Objects/Levels/Emerald Coast/Whale.sa1mdl");
			whalemsh = ObjectHelper.GetMeshes(whale, dev);
			sphere = ObjectHelper.LoadModel("Objects/Collision/C SPHERE.sa1mdl");
			spheremsh = ObjectHelper.GetMeshes(sphere, dev);
		}

		public override string Name { get { return "Whale Spawner"; } }
	}

	public abstract class O_AOKill : ObjectDefinition
	{
		protected NJS_OBJECT whale;
		protected Mesh[] whalemsh;
		protected NJS_OBJECT sphere;
		protected Mesh[] spheremsh;

		public override HitResult CheckHit(SETItem item, Vector3 Near, Vector3 Far, Viewport Viewport, Matrix Projection, Matrix View, MatrixStack transform)
		{
			HitResult result = HitResult.NoHit;
			transform.Push();
			transform.NJTranslate(item.Position);
			transform.NJScale(4.5f, 4.5f, 4.5f);
			transform.Push();
			result = HitResult.Min(result, sphere.CheckHit(Near, Far, Viewport, Projection, View, transform, spheremsh));
			transform.Pop();
			transform.Push();
			transform.NJTranslate(item.Position);
			transform.NJRotateZ(0x8000);
			transform.NJScale(0.40000001f, 0.40000001f, 0.40000001f);
			transform.Push();
			result = HitResult.Min(result, whale.CheckHit(Near, Far, Viewport, Projection, View, transform, whalemsh));
			transform.Pop();
			return result;
		}

		public override List<RenderInfo> Render(SETItem item, Device dev, EditorCamera camera, MatrixStack transform)
		{
			List<RenderInfo> result = new List<RenderInfo>();
			transform.Push();
			transform.NJTranslate(item.Position);
			transform.NJScale(4.5f, 4.5f, 4.5f);
			result.AddRange(sphere.DrawModelTree(dev, transform, null, spheremsh));
			if (item.Selected)
				result.AddRange(sphere.DrawModelTreeInvert(transform, spheremsh));
			transform.Pop();
			transform.Push();
			transform.NJTranslate(item.Position);
			transform.NJRotateZ(0x8000);
			transform.NJScale(0.40000001f, 0.40000001f, 0.40000001f);
			result.AddRange(whale.DrawModelTree(dev, transform, ObjectHelper.GetTextures("OBJ_BEACH"), whalemsh));
			if (item.Selected)
				result.AddRange(whale.DrawModelTreeInvert(transform, whalemsh));
			transform.Pop();
			return result;
		}

		public override BoundingSphere GetBounds(SETItem item)
		{
			MatrixStack transform = new MatrixStack();
			transform.NJTranslate(item.Position);
			transform.NJScale(4.5f, 4.5f, 4.5f);
			return ObjectHelper.GetModelBounds(sphere, transform);
		}
	}

	public class AOKill : O_AOKill
	{
		public override void Init(ObjectData data, string name, Device dev)
		{
			whale = ObjectHelper.LoadModel("Objects/Levels/Emerald Coast/Whale.sa1mdl");
			whalemsh = ObjectHelper.GetMeshes(whale, dev);
			sphere = ObjectHelper.LoadModel("Objects/Collision/C SPHERE.sa1mdl");
			spheremsh = ObjectHelper.GetMeshes(sphere, dev);
		}

		public override string Name { get { return "Whale Despawner"; } }
	}

	public abstract class O_POSummon : ObjectDefinition
	{
		protected NJS_OBJECT whale;
		protected Mesh[] whalemsh;
		protected NJS_OBJECT sphere;
		protected Mesh[] spheremsh;

		public override HitResult CheckHit(SETItem item, Vector3 Near, Vector3 Far, Viewport Viewport, Matrix Projection, Matrix View, MatrixStack transform)
		{
			HitResult result = HitResult.NoHit;
			transform.Push();
			transform.NJTranslate(item.Position);
			transform.NJScale(4.5f, 4.5f, 4.5f);
			transform.Push();
			result = HitResult.Min(result, sphere.CheckHit(Near, Far, Viewport, Projection, View, transform, spheremsh));
			transform.Pop();
			transform.Push();
			transform.NJTranslate(item.Position);
			transform.NJRotateX(0x2000);
			transform.NJScale(0.40000001f, 0.40000001f, 0.40000001f);
			transform.Push();
			result = HitResult.Min(result, whale.CheckHit(Near, Far, Viewport, Projection, View, transform, whalemsh));
			transform.Pop();
			return result;
		}

		public override List<RenderInfo> Render(SETItem item, Device dev, EditorCamera camera, MatrixStack transform)
		{
			List<RenderInfo> result = new List<RenderInfo>();
			transform.Push();
			transform.NJTranslate(item.Position);
			transform.NJScale(4.5f, 4.5f, 4.5f);
			result.AddRange(sphere.DrawModelTree(dev, transform, null, spheremsh));
			if (item.Selected)
				result.AddRange(sphere.DrawModelTreeInvert(transform, spheremsh));
			transform.Pop();
			transform.Push();
			transform.NJTranslate(item.Position);
			transform.NJRotateX(0x2000);
			transform.NJScale(0.40000001f, 0.40000001f, 0.40000001f);
			result.AddRange(whale.DrawModelTree(dev, transform, ObjectHelper.GetTextures("OBJ_BEACH"), whalemsh));
			if (item.Selected)
				result.AddRange(whale.DrawModelTreeInvert(transform, whalemsh));
			transform.Pop();
			return result;
		}

		public override BoundingSphere GetBounds(SETItem item)
		{
			MatrixStack transform = new MatrixStack();
			transform.NJTranslate(item.Position);
			transform.NJScale(4.5f, 4.5f, 4.5f);
			return ObjectHelper.GetModelBounds(sphere, transform);
		}
	}

	public class POSummon : O_POSummon
	{
		public override void Init(ObjectData data, string name, Device dev)
		{
			whale = ObjectHelper.LoadModel("Objects/Levels/Emerald Coast/Whale.sa1mdl");
			whalemsh = ObjectHelper.GetMeshes(whale, dev);
			sphere = ObjectHelper.LoadModel("Objects/Collision/C SPHERE.sa1mdl");
			spheremsh = ObjectHelper.GetMeshes(sphere, dev);
		}

		public override string Name { get { return "PO Whale Spawner"; } }
	}
}