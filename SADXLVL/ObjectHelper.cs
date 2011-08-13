﻿using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using SonicRetro.SAModel.Direct3D;

namespace SonicRetro.SAModel.SADXLVL2
{
    public static class ObjectHelper
    {
        internal static CustomVertex.PositionTextured[] Square = {
        new CustomVertex.PositionTextured(-8, 8, 0, 1, 0),
        new CustomVertex.PositionTextured(-8, -8, 0, 1, 1),
        new CustomVertex.PositionTextured(8, 8, 0, 0, 0),
        new CustomVertex.PositionTextured(-8, -8, 0, 1, 1),
        new CustomVertex.PositionTextured(8, -8, 0, 0, 1),
        new CustomVertex.PositionTextured(8, 8, 0, 0, 0)};

        internal static Texture QuestionMark;

        public static Object LoadModel(string file)
        {
            Dictionary<string, Dictionary<string, string>> mdlini = IniFile.Load(file);
            return new Object(mdlini, mdlini[string.Empty]["Root"]);
        }

        public static Microsoft.DirectX.Direct3D.Mesh[] GetMeshes(Object model, Device dev)
        {
            Object[] models = model.GetObjects();
            Microsoft.DirectX.Direct3D.Mesh[] Meshes = new Microsoft.DirectX.Direct3D.Mesh[models.Length];
            for (int i = 0; i < models.Length; i++)
                if (models[i].Attach != null)
                    Meshes[i] = models[i].Attach.CreateD3DMesh(dev);
            return Meshes;
        }

        public static Texture[] GetTextures(string name)
        {
            if (LevelData.Textures.ContainsKey(name))
                return LevelData.Textures[name];
            return null;
        }

        public static float CheckSpriteHit(Vector3 Near, Vector3 Far, Viewport Viewport, Matrix Projection, Matrix View, MatrixStack transform)
        {
            Vector3 pos = Vector3.Unproject(Near, Viewport, Projection, View, transform.Top);
            Vector3 dir = Vector3.Subtract(pos, Vector3.Unproject(Far, Viewport, Projection, View, transform.Top));
            float dist = -1;
            for (int i = 0; i < 2; i++)
            {
                IntersectInformation info;
                if (Geometry.IntersectTri(Square[i * 3].Position, Square[i * 3 + 1].Position, Square[i * 3 + 2].Position, pos, dir, out info))
                {
                    if (dist == -1)
                        dist = info.Dist;
                    else if (dist > info.Dist)
                        dist = info.Dist;
                }
            }
            return dist;
        }

        public static void RenderSprite(Device dev, MatrixStack transform, Texture texture, bool selected)
        {
            VertexFormats fmt = dev.VertexFormat;
            dev.VertexFormat = CustomVertex.PositionTextured.Format;
            dev.Material = new Microsoft.DirectX.Direct3D.Material
            {
                Diffuse = Color.White,
                Ambient = Color.White
            };
            dev.TextureState[0].TextureCoordinateIndex = 0;
            TextureFilter magfilter = dev.SamplerState[0].MagFilter;
            TextureFilter minfilter = dev.SamplerState[0].MinFilter;
            TextureFilter mipfilter = dev.SamplerState[0].MipFilter;
            dev.SamplerState[0].MagFilter = TextureFilter.None;
            dev.SamplerState[0].MinFilter = TextureFilter.None;
            dev.SamplerState[0].MipFilter = TextureFilter.None;
            dev.Transform.World = transform.Top;
            if (texture == null)
                dev.SetTexture(0, QuestionMark);
            else
                dev.SetTexture(0, texture);
            dev.DrawUserPrimitives(PrimitiveType.TriangleList, 2, Square);
            if (selected)
            {
                dev.Material = new Microsoft.DirectX.Direct3D.Material
                {
                    Diffuse = Color.Yellow,
                    Ambient = Color.Yellow
                };
                FillMode mode = dev.RenderState.FillMode;
                dev.RenderState.FillMode = FillMode.WireFrame;
                dev.DrawUserPrimitives(PrimitiveType.TriangleList, 2, Square);
                dev.RenderState.FillMode = mode;
            }
            dev.SamplerState[0].MagFilter = magfilter;
            dev.SamplerState[0].MinFilter = minfilter;
            dev.SamplerState[0].MipFilter = mipfilter;
            dev.VertexFormat = fmt;
        }

        public static float BAMSToRad(int BAMS)
        {
            return (float)(BAMS / (65536 / (2 * Math.PI)));
        }

        public static int RadToBAMS(float rad)
        {
            return (int)(rad * (65536 / (2 * Math.PI)));
        }

        public static float BAMSToDeg(int BAMS)
        {
            return (float)(BAMS / (65536 / 360.0));
        }

        public static int DegToBAMS(float deg)
        {
            return (int)(deg * (65536 / 360.0));
        }

        private static float[] BAMSTable = {
                                               0.0F,
                                               0.001534F,
                                               0.0030680001F,
                                               0.0046020001F,
                                               0.0061360002F,
                                               0.0076700002F,
                                               0.0092040002F,
                                               0.010738F,
                                               0.012272F,
                                               0.013805F,
                                               0.015339F,
                                               0.016873F,
                                               0.018407F,
                                               0.01994F,
                                               0.021474F,
                                               0.023008F,
                                               0.024541F,
                                               0.026075F,
                                               0.027608F,
                                               0.029142F,
                                               0.030675F,
                                               0.032207999F,
                                               0.033741001F,
                                               0.035273999F,
                                               0.036807001F,
                                               0.038339999F,
                                               0.039873F,
                                               0.041405998F,
                                               0.042938001F,
                                               0.044470999F,
                                               0.046002999F,
                                               0.047534999F,
                                               0.049068F,
                                               0.0506F,
                                               0.052131999F,
                                               0.053663999F,
                                               0.055195F,
                                               0.056727F,
                                               0.058258001F,
                                               0.05979F,
                                               0.061321001F,
                                               0.062852003F,
                                               0.064383F,
                                               0.065912999F,
                                               0.067443997F,
                                               0.068974003F,
                                               0.070505001F,
                                               0.072035F,
                                               0.073564999F,
                                               0.075094F,
                                               0.076623999F,
                                               0.078152999F,
                                               0.079682F,
                                               0.081211001F,
                                               0.082740001F,
                                               0.084269002F,
                                               0.085796997F,
                                               0.087325998F,
                                               0.088854F,
                                               0.090380996F,
                                               0.091908999F,
                                               0.093436003F,
                                               0.094962999F,
                                               0.096490003F,
                                               0.098017F,
                                               0.099544004F,
                                               0.10107F,
                                               0.102596F,
                                               0.104122F,
                                               0.105647F,
                                               0.107172F,
                                               0.108697F,
                                               0.110222F,
                                               0.111747F,
                                               0.113271F,
                                               0.114795F,
                                               0.116319F,
                                               0.117842F,
                                               0.119365F,
                                               0.120888F,
                                               0.122411F,
                                               0.123933F,
                                               0.12545501F,
                                               0.126977F,
                                               0.128498F,
                                               0.13001899F,
                                               0.13154F,
                                               0.13306101F,
                                               0.134581F,
                                               0.13610099F,
                                               0.13762F,
                                               0.139139F,
                                               0.14065801F,
                                               0.142177F,
                                               0.143695F,
                                               0.14521299F,
                                               0.14673001F,
                                               0.148248F,
                                               0.149765F,
                                               0.151281F,
                                               0.152797F,
                                               0.154313F,
                                               0.155828F,
                                               0.157343F,
                                               0.158858F,
                                               0.160372F,
                                               0.16188601F,
                                               0.16339999F,
                                               0.164913F,
                                               0.166426F,
                                               0.16793799F,
                                               0.16945F,
                                               0.17096201F,
                                               0.172473F,
                                               0.17398401F,
                                               0.175494F,
                                               0.17700399F,
                                               0.178514F,
                                               0.180023F,
                                               0.181532F,
                                               0.18303999F,
                                               0.18454801F,
                                               0.186055F,
                                               0.187562F,
                                               0.189069F,
                                               0.190575F,
                                               0.19208001F,
                                               0.19358601F,
                                               0.19509F,
                                               0.196595F,
                                               0.198098F,
                                               0.19960199F,
                                               0.201105F,
                                               0.20260701F,
                                               0.204109F,
                                               0.20561001F,
                                               0.207111F,
                                               0.20861199F,
                                               0.21011201F,
                                               0.211611F,
                                               0.21311F,
                                               0.214609F,
                                               0.216107F,
                                               0.217604F,
                                               0.219101F,
                                               0.220598F,
                                               0.222094F,
                                               0.223589F,
                                               0.22508401F,
                                               0.226578F,
                                               0.228072F,
                                               0.22956499F,
                                               0.231058F,
                                               0.23255F,
                                               0.234042F,
                                               0.235533F,
                                               0.23702399F,
                                               0.23851401F,
                                               0.240003F,
                                               0.241492F,
                                               0.24298F,
                                               0.244468F,
                                               0.24595501F,
                                               0.24744201F,
                                               0.248928F,
                                               0.250413F,
                                               0.25189799F,
                                               0.253382F,
                                               0.254866F,
                                               0.256349F,
                                               0.25783101F,
                                               0.25931299F,
                                               0.26079401F,
                                               0.26227501F,
                                               0.26375499F,
                                               0.26523399F,
                                               0.26671299F,
                                               0.26819101F,
                                               0.26966801F,
                                               0.27114499F,
                                               0.27262101F,
                                               0.274097F,
                                               0.275572F,
                                               0.27704599F,
                                               0.27851999F,
                                               0.279993F,
                                               0.28146499F,
                                               0.28293699F,
                                               0.284408F,
                                               0.285878F,
                                               0.28734699F,
                                               0.288816F,
                                               0.29028499F,
                                               0.29175201F,
                                               0.293219F,
                                               0.29468501F,
                                               0.29615101F,
                                               0.297616F,
                                               0.29908001F,
                                               0.30054301F,
                                               0.30200601F,
                                               0.30346799F,
                                               0.30492899F,
                                               0.30638999F,
                                               0.30785F,
                                               0.30930901F,
                                               0.31076699F,
                                               0.31222501F,
                                               0.31368199F,
                                               0.31513801F,
                                               0.31659299F,
                                               0.318048F,
                                               0.319502F,
                                               0.32095501F,
                                               0.32240799F,
                                               0.32385901F,
                                               0.32530999F,
                                               0.32675999F,
                                               0.32821F,
                                               0.329658F,
                                               0.33110601F,
                                               0.332553F,
                                               0.33399999F,
                                               0.33544499F,
                                               0.33689001F,
                                               0.33833399F,
                                               0.33977699F,
                                               0.34121901F,
                                               0.34266099F,
                                               0.34410101F,
                                               0.345541F,
                                               0.34698001F,
                                               0.34841901F,
                                               0.34985599F,
                                               0.351293F,
                                               0.35272899F,
                                               0.354164F,
                                               0.355598F,
                                               0.35703099F,
                                               0.35846299F,
                                               0.35989499F,
                                               0.36132601F,
                                               0.36275601F,
                                               0.36418501F,
                                               0.36561301F,
                                               0.36704001F,
                                               0.368467F,
                                               0.369892F,
                                               0.371317F,
                                               0.37274101F,
                                               0.37416399F,
                                               0.375586F,
                                               0.37700701F,
                                               0.37842801F,
                                               0.37984699F,
                                               0.381266F,
                                               0.38268301F,
                                               0.38409999F,
                                               0.38551599F,
                                               0.386931F,
                                               0.388345F,
                                               0.38975799F,
                                               0.39117F,
                                               0.392582F,
                                               0.39399201F,
                                               0.395401F,
                                               0.39681F,
                                               0.39821801F,
                                               0.39962399F,
                                               0.40103F,
                                               0.402435F,
                                               0.40383801F,
                                               0.40524101F,
                                               0.406643F,
                                               0.40804401F,
                                               0.409444F,
                                               0.41084301F,
                                               0.41224101F,
                                               0.413638F,
                                               0.415034F,
                                               0.41643F,
                                               0.417824F,
                                               0.41921699F,
                                               0.420609F,
                                               0.42199999F,
                                               0.42339F,
                                               0.42478001F,
                                               0.42616799F,
                                               0.42755499F,
                                               0.42894101F,
                                               0.43032601F,
                                               0.43171099F,
                                               0.43309399F,
                                               0.43447599F,
                                               0.435857F,
                                               0.43723699F,
                                               0.43861601F,
                                               0.43999401F,
                                               0.44137099F,
                                               0.442747F,
                                               0.44412199F,
                                               0.44549599F,
                                               0.44686899F,
                                               0.448241F,
                                               0.44961101F,
                                               0.45098099F,
                                               0.45234999F,
                                               0.45371699F,
                                               0.455084F,
                                               0.456449F,
                                               0.45781299F,
                                               0.45917699F,
                                               0.46053901F,
                                               0.4619F,
                                               0.46325999F,
                                               0.46461901F,
                                               0.46597701F,
                                               0.46733299F,
                                               0.46868899F,
                                               0.470043F,
                                               0.47139701F,
                                               0.47274899F,
                                               0.47409999F,
                                               0.47545001F,
                                               0.47679901F,
                                               0.478147F,
                                               0.47949401F,
                                               0.48083901F,
                                               0.48218399F,
                                               0.483527F,
                                               0.484869F,
                                               0.48620999F,
                                               0.48754999F,
                                               0.48888901F,
                                               0.49022701F,
                                               0.49156299F,
                                               0.49289799F,
                                               0.494232F,
                                               0.495565F,
                                               0.49689701F,
                                               0.49822801F,
                                               0.49955699F,
                                               0.50088501F,
                                               0.50221199F,
                                               0.50353801F,
                                               0.50486302F,
                                               0.50618702F,
                                               0.50750899F,
                                               0.50883001F,
                                               0.51015002F,
                                               0.51146901F,
                                               0.51278597F,
                                               0.514103F,
                                               0.51541799F,
                                               0.51673198F,
                                               0.51804399F,
                                               0.51935601F,
                                               0.520666F,
                                               0.52197498F,
                                               0.523283F,
                                               0.52459002F,
                                               0.525895F,
                                               0.52719897F,
                                               0.52850199F,
                                               0.52980399F,
                                               0.53110403F,
                                               0.53240299F,
                                               0.533701F,
                                               0.534998F,
                                               0.53629303F,
                                               0.53758699F,
                                               0.53887999F,
                                               0.54017198F,
                                               0.541462F,
                                               0.54275101F,
                                               0.54403901F,
                                               0.54532498F,
                                               0.54661F,
                                               0.547894F,
                                               0.54917699F,
                                               0.55045801F,
                                               0.55173802F,
                                               0.55301702F,
                                               0.55429399F,
                                               0.55557001F,
                                               0.55684501F,
                                               0.558119F,
                                               0.55939102F,
                                               0.56066197F,
                                               0.56193101F,
                                               0.56319898F,
                                               0.564466F,
                                               0.565732F,
                                               0.56699598F,
                                               0.568259F,
                                               0.56952101F,
                                               0.57078099F,
                                               0.57204002F,
                                               0.57329702F,
                                               0.57455301F,
                                               0.57580799F,
                                               0.57706201F,
                                               0.57831401F,
                                               0.57956499F,
                                               0.580814F,
                                               0.58206201F,
                                               0.58330899F,
                                               0.58455402F,
                                               0.58579803F,
                                               0.58704001F,
                                               0.58828199F,
                                               0.58952099F,
                                               0.59075999F,
                                               0.59199703F,
                                               0.59323198F,
                                               0.59446698F,
                                               0.59569901F,
                                               0.59693098F,
                                               0.59816098F,
                                               0.59938902F,
                                               0.60061598F,
                                               0.60184199F,
                                               0.60306698F,
                                               0.60429001F,
                                               0.60551101F,
                                               0.606731F,
                                               0.60794997F,
                                               0.60916698F,
                                               0.61038297F,
                                               0.611597F,
                                               0.61281002F,
                                               0.61402202F,
                                               0.61523199F,
                                               0.61644F,
                                               0.61764699F,
                                               0.61885297F,
                                               0.62005699F,
                                               0.62125999F,
                                               0.62246102F,
                                               0.62366098F,
                                               0.62485999F,
                                               0.62605602F,
                                               0.62725198F,
                                               0.62844598F,
                                               0.62963802F,
                                               0.63082898F,
                                               0.63201898F,
                                               0.63320702F,
                                               0.63439298F,
                                               0.63557798F,
                                               0.63676202F,
                                               0.63794398F,
                                               0.63912398F,
                                               0.64030302F,
                                               0.64148098F,
                                               0.64265698F,
                                               0.64383203F,
                                               0.64500499F,
                                               0.64617598F,
                                               0.64734602F,
                                               0.64851397F,
                                               0.64968097F,
                                               0.65084702F,
                                               0.65201098F,
                                               0.65317303F,
                                               0.65433401F,
                                               0.65549302F,
                                               0.65665102F,
                                               0.65780699F,
                                               0.658961F,
                                               0.66011399F,
                                               0.66126603F,
                                               0.66241598F,
                                               0.66356403F,
                                               0.664711F,
                                               0.665856F,
                                               0.667F,
                                               0.66814202F,
                                               0.66928297F,
                                               0.67042202F,
                                               0.67155898F,
                                               0.67269498F,
                                               0.67382902F,
                                               0.67496198F,
                                               0.67609298F,
                                               0.67722201F,
                                               0.67834997F,
                                               0.67947602F,
                                               0.680601F,
                                               0.68172401F,
                                               0.68284601F,
                                               0.68396503F,
                                               0.68508399F,
                                               0.68620002F,
                                               0.68731499F,
                                               0.688429F,
                                               0.68954098F,
                                               0.690651F,
                                               0.69175899F,
                                               0.69286603F,
                                               0.69397098F,
                                               0.69507498F,
                                               0.69617701F,
                                               0.69727701F,
                                               0.698376F,
                                               0.69947302F,
                                               0.70056897F,
                                               0.70166302F,
                                               0.70275497F,
                                               0.70384502F,
                                               0.704934F,
                                               0.70602101F,
                                               0.70710701F,
                                               0.70819098F,
                                               0.70927298F,
                                               0.71035302F,
                                               0.71143198F,
                                               0.71250898F,
                                               0.71358502F,
                                               0.71465898F,
                                               0.71573102F,
                                               0.71680099F,
                                               0.71787F,
                                               0.71893698F,
                                               0.72000301F,
                                               0.721066F,
                                               0.72212797F,
                                               0.723189F,
                                               0.72424698F,
                                               0.72530401F,
                                               0.72635901F,
                                               0.727413F,
                                               0.72846401F,
                                               0.729514F,
                                               0.73056298F,
                                               0.73160899F,
                                               0.73265398F,
                                               0.733697F,
                                               0.73473901F,
                                               0.73577899F,
                                               0.736817F,
                                               0.73785299F,
                                               0.73888701F,
                                               0.73992002F,
                                               0.740951F,
                                               0.74198002F,
                                               0.74300802F,
                                               0.74403399F,
                                               0.745058F,
                                               0.74607998F,
                                               0.74710101F,
                                               0.748119F,
                                               0.74913597F,
                                               0.75015199F,
                                               0.75116497F,
                                               0.752177F,
                                               0.753187F,
                                               0.75419497F,
                                               0.75520098F,
                                               0.75620598F,
                                               0.757209F,
                                               0.75821F,
                                               0.75920898F,
                                               0.760207F,
                                               0.76120198F,
                                               0.762196F,
                                               0.763188F,
                                               0.76417899F,
                                               0.765167F,
                                               0.76615399F,
                                               0.76713902F,
                                               0.76812202F,
                                               0.76910299F,
                                               0.77008301F,
                                               0.771061F,
                                               0.77203602F,
                                               0.77301002F,
                                               0.773983F,
                                               0.77495301F,
                                               0.775922F,
                                               0.77688801F,
                                               0.77785301F,
                                               0.778817F,
                                               0.779778F,
                                               0.78073698F,
                                               0.78169501F,
                                               0.78265101F,
                                               0.78360498F,
                                               0.78455698F,
                                               0.78550702F,
                                               0.78645498F,
                                               0.78740197F,
                                               0.78834599F,
                                               0.789289F,
                                               0.79022998F,
                                               0.79116899F,
                                               0.79210699F,
                                               0.793042F,
                                               0.793975F,
                                               0.79490697F,
                                               0.79583699F,
                                               0.79676503F,
                                               0.79769099F,
                                               0.79861498F,
                                               0.799537F,
                                               0.80045801F,
                                               0.80137599F,
                                               0.802293F,
                                               0.80320799F,
                                               0.80412F,
                                               0.805031F,
                                               0.80593997F,
                                               0.80684799F,
                                               0.80775303F,
                                               0.80865598F,
                                               0.80955797F,
                                               0.81045699F,
                                               0.81135499F,
                                               0.81225097F,
                                               0.81314403F,
                                               0.81403601F,
                                               0.81492603F,
                                               0.81581402F,
                                               0.81670099F,
                                               0.81758499F,
                                               0.81846702F,
                                               0.81934798F,
                                               0.82022601F,
                                               0.82110202F,
                                               0.82197702F,
                                               0.82284999F,
                                               0.82372099F,
                                               0.82458901F,
                                               0.82545602F,
                                               0.82632101F,
                                               0.82718402F,
                                               0.82804501F,
                                               0.82890397F,
                                               0.82976103F,
                                               0.830616F,
                                               0.83147001F,
                                               0.83232099F,
                                               0.83317F,
                                               0.83401799F,
                                               0.83486301F,
                                               0.835706F,
                                               0.83654797F,
                                               0.83738703F,
                                               0.83822501F,
                                               0.83906001F,
                                               0.839894F,
                                               0.840725F,
                                               0.841555F,
                                               0.84238303F,
                                               0.84320801F,
                                               0.84403199F,
                                               0.844854F,
                                               0.84567302F,
                                               0.84649098F,
                                               0.84730703F,
                                               0.84811997F,
                                               0.84893203F,
                                               0.849742F,
                                               0.85055F,
                                               0.85135502F,
                                               0.85215902F,
                                               0.852961F,
                                               0.85376F,
                                               0.85455799F,
                                               0.85535401F,
                                               0.85614699F,
                                               0.85693902F,
                                               0.85772902F,
                                               0.85851598F,
                                               0.85930198F,
                                               0.86008501F,
                                               0.86086702F,
                                               0.861646F,
                                               0.86242402F,
                                               0.863199F,
                                               0.86397302F,
                                               0.86474401F,
                                               0.86551398F,
                                               0.86628097F,
                                               0.867046F,
                                               0.867809F,
                                               0.86857098F,
                                               0.86932999F,
                                               0.87008703F,
                                               0.87084198F,
                                               0.87159503F,
                                               0.87234598F,
                                               0.87309498F,
                                               0.873842F,
                                               0.874587F,
                                               0.87532902F,
                                               0.87607002F,
                                               0.876809F,
                                               0.877545F,
                                               0.87827998F,
                                               0.87901199F,
                                               0.87974298F,
                                               0.88047099F,
                                               0.88119698F,
                                               0.88192099F,
                                               0.88264298F,
                                               0.88336301F,
                                               0.88408101F,
                                               0.88479698F,
                                               0.88551098F,
                                               0.88622302F,
                                               0.88693202F,
                                               0.88764F,
                                               0.888345F,
                                               0.88904798F,
                                               0.88975F,
                                               0.89044899F,
                                               0.891146F,
                                               0.89184099F,
                                               0.89253402F,
                                               0.893224F,
                                               0.89391297F,
                                               0.89459902F,
                                               0.895284F,
                                               0.89596599F,
                                               0.89664602F,
                                               0.89732498F,
                                               0.89800102F,
                                               0.89867401F,
                                               0.89934599F,
                                               0.90001601F,
                                               0.90068299F,
                                               0.90134901F,
                                               0.90201199F,
                                               0.90267301F,
                                               0.903332F,
                                               0.90398902F,
                                               0.90464401F,
                                               0.90529698F,
                                               0.90594703F,
                                               0.906596F,
                                               0.907242F,
                                               0.90788603F,
                                               0.90852797F,
                                               0.909168F,
                                               0.90980601F,
                                               0.91044098F,
                                               0.911075F,
                                               0.91170597F,
                                               0.91233498F,
                                               0.91296202F,
                                               0.91358697F,
                                               0.91421002F,
                                               0.91483003F,
                                               0.91544902F,
                                               0.91606498F,
                                               0.91667902F,
                                               0.91729099F,
                                               0.91790098F,
                                               0.91850799F,
                                               0.91911399F,
                                               0.91971701F,
                                               0.92031801F,
                                               0.92091697F,
                                               0.92151397F,
                                               0.92210901F,
                                               0.922701F,
                                               0.92329103F,
                                               0.92387998F,
                                               0.924465F,
                                               0.92504901F,
                                               0.92563099F,
                                               0.92620999F,
                                               0.92678702F,
                                               0.92736298F,
                                               0.927935F,
                                               0.92850602F,
                                               0.929075F,
                                               0.92964101F,
                                               0.93020499F,
                                               0.930767F,
                                               0.93132699F,
                                               0.93188399F,
                                               0.93243998F,
                                               0.93299299F,
                                               0.93354398F,
                                               0.934093F,
                                               0.93463898F,
                                               0.935184F,
                                               0.93572599F,
                                               0.93626601F,
                                               0.93680298F,
                                               0.93733901F,
                                               0.93787199F,
                                               0.93840402F,
                                               0.938932F,
                                               0.93945903F,
                                               0.93998402F,
                                               0.94050598F,
                                               0.94102597F,
                                               0.941544F,
                                               0.94205999F,
                                               0.94257301F,
                                               0.943084F,
                                               0.94359303F,
                                               0.94410002F,
                                               0.94460499F,
                                               0.94510698F,
                                               0.94560701F,
                                               0.946105F,
                                               0.94660097F,
                                               0.94709402F,
                                               0.947586F,
                                               0.948075F,
                                               0.94856101F,
                                               0.94904602F,
                                               0.94952798F,
                                               0.95000798F,
                                               0.950486F,
                                               0.95096201F,
                                               0.95143503F,
                                               0.95190603F,
                                               0.95237499F,
                                               0.952842F,
                                               0.95330602F,
                                               0.95376801F,
                                               0.95422798F,
                                               0.95468599F,
                                               0.95514101F,
                                               0.955594F,
                                               0.95604497F,
                                               0.95649397F,
                                               0.95694F,
                                               0.957385F,
                                               0.95782602F,
                                               0.95826602F,
                                               0.95870298F,
                                               0.95913899F,
                                               0.959571F,
                                               0.96000201F,
                                               0.96043098F,
                                               0.96085697F,
                                               0.96127999F,
                                               0.96170199F,
                                               0.96212101F,
                                               0.962538F,
                                               0.96295297F,
                                               0.96336597F,
                                               0.96377599F,
                                               0.96418399F,
                                               0.96459001F,
                                               0.964993F,
                                               0.96539402F,
                                               0.96579301F,
                                               0.96618998F,
                                               0.96658403F,
                                               0.96697599F,
                                               0.96736598F,
                                               0.96775401F,
                                               0.96813899F,
                                               0.96852201F,
                                               0.96890301F,
                                               0.96928102F,
                                               0.969657F,
                                               0.97003102F,
                                               0.97040302F,
                                               0.97077203F,
                                               0.97113901F,
                                               0.97150397F,
                                               0.97186601F,
                                               0.97222698F,
                                               0.97258401F,
                                               0.97294003F,
                                               0.97329301F,
                                               0.97364402F,
                                               0.973993F,
                                               0.97433901F,
                                               0.974684F,
                                               0.975025F,
                                               0.97536498F,
                                               0.97570199F,
                                               0.97603703F,
                                               0.97636998F,
                                               0.97670001F,
                                               0.97702801F,
                                               0.97735399F,
                                               0.97767699F,
                                               0.97799802F,
                                               0.97831702F,
                                               0.978634F,
                                               0.978948F,
                                               0.97926003F,
                                               0.97956997F,
                                               0.979877F,
                                               0.98018199F,
                                               0.98048502F,
                                               0.98078501F,
                                               0.98108298F,
                                               0.98137897F,
                                               0.981673F,
                                               0.98196399F,
                                               0.98225302F,
                                               0.982539F,
                                               0.98282403F,
                                               0.983105F,
                                               0.98338503F,
                                               0.98366201F,
                                               0.98393703F,
                                               0.98421001F,
                                               0.98448002F,
                                               0.98474801F,
                                               0.98501402F,
                                               0.98527801F,
                                               0.98553902F,
                                               0.985798F,
                                               0.986054F,
                                               0.98630798F,
                                               0.98655999F,
                                               0.98680902F,
                                               0.98705697F,
                                               0.98730099F,
                                               0.987544F,
                                               0.98778403F,
                                               0.98802203F,
                                               0.988258F,
                                               0.988491F,
                                               0.98872203F,
                                               0.98895001F,
                                               0.98917699F,
                                               0.98940003F,
                                               0.989622F,
                                               0.98984098F,
                                               0.990058F,
                                               0.990273F,
                                               0.99048501F,
                                               0.990695F,
                                               0.99090302F,
                                               0.991108F,
                                               0.99131101F,
                                               0.99151099F,
                                               0.99171001F,
                                               0.99190599F,
                                               0.99209899F,
                                               0.99229097F,
                                               0.99247998F,
                                               0.99266601F,
                                               0.99285001F,
                                               0.99303198F,
                                               0.99321198F,
                                               0.99338901F,
                                               0.99356401F,
                                               0.99373698F,
                                               0.99390697F,
                                               0.994075F,
                                               0.99423999F,
                                               0.99440402F,
                                               0.99456501F,
                                               0.99472302F,
                                               0.99487901F,
                                               0.99503303F,
                                               0.99518502F,
                                               0.99533403F,
                                               0.99548101F,
                                               0.99562502F,
                                               0.995767F,
                                               0.99590701F,
                                               0.99604499F,
                                               0.99618F,
                                               0.99631298F,
                                               0.99644297F,
                                               0.996571F,
                                               0.99669701F,
                                               0.99681997F,
                                               0.99694097F,
                                               0.99706F,
                                               0.99717599F,
                                               0.99729002F,
                                               0.99740201F,
                                               0.99751103F,
                                               0.99761802F,
                                               0.99772298F,
                                               0.99782503F,
                                               0.99792498F,
                                               0.99802297F,
                                               0.99811798F,
                                               0.99821103F,
                                               0.99830198F,
                                               0.99839002F,
                                               0.99847603F,
                                               0.998559F,
                                               0.99864F,
                                               0.99871898F,
                                               0.99879497F,
                                               0.99887002F,
                                               0.998941F,
                                               0.99901098F,
                                               0.99907798F,
                                               0.99914199F,
                                               0.99920499F,
                                               0.99926502F,
                                               0.999322F,
                                               0.99937803F,
                                               0.99943101F,
                                               0.99948102F,
                                               0.999529F,
                                               0.99957502F,
                                               0.99961901F,
                                               0.99966002F,
                                               0.999699F,
                                               0.999735F,
                                               0.99976897F,
                                               0.99980098F,
                                               0.99983102F,
                                               0.99985802F,
                                               0.99988198F,
                                               0.99990499F,
                                               0.99992502F,
                                               0.999942F,
                                               0.99995798F,
                                               0.99997097F,
                                               0.99998099F,
                                               0.99998897F,
                                               0.99999499F,
                                               0.99999899F,
                                               1.0F
                                           };

        public static float ConvertBAMS(int BAMS)
        {
            int v1;
            int v2;
            int v3;
            byte v4;
            double v6;
            double v7;
            float v8;
            int a1 = BAMS;
            v8 = a1;
            v4 = (byte)a1;
            v3 = (a1 >> 4) & 0xFFF;
            v2 = v4 & 0xF;
            v1 = v3 & 0xC00;
            if (v2 != 0)
            {
                if (v1 > 0x800)
                {
                    if (v1 == 0xC00)
                    {
                        v7 = -BAMSTable[4096 - v3];
                        v6 = -BAMSTable[4096 - v3 + 1];
                        return (float)(v7 + (v6 - v7) * v2 * 0.0625);
                    }
                }
                else
                {
                    if (v1 == 0x800)
                    {
                        v7 = -BAMSTable[-2048 + v3];
                        v6 = -BAMSTable[-2048 + v3 + 1];
                        return (float)(v7 + (v6 - v7) * v2 * 0.0625);
                    }
                    if ((v3 & 0xC00) == 0)
                    {
                        v7 = BAMSTable[v3];
                        v6 = BAMSTable[1 + v3];
                        return (float)(v7 + (v6 - v7) * v2 * 0.0625);
                    }
                    if (v1 == 0x400)
                    {
                        v7 = BAMSTable[2048 - v3];
                        v6 = BAMSTable[2048 - v3 + 1];
                        return (float)(v7 + (v6 - v7) * v2 * 0.0625);
                    }
                }
                v7 = v8;
                v6 = v8;
                return (float)(v7 + (v6 - v7) * v2 * 0.0625);
            }
            if (v1 > 0x800)
            {
                if (v1 == 0xC00)
                    return -BAMSTable[4096 - v3];
            }
            else
            {
                if (v1 == 0x800)
                    return -BAMSTable[-2048 + v3];
                if ((v3 & 0xC00) == 0)
                    return BAMSTable[v3];
                if (v1 == 0x400)
                    return BAMSTable[2048 - v3];
            }
            return v8;
        }

        public static float ConvertBAMSInv(int BAMS)
        {
            int v1;
            int v2;
            int v3;
            byte v4;
            double v6;
            double v7;
            float v8;
            int a1 = BAMS;
            v8 = a1;
            v4 = (byte)a1;
            v3 = (a1 >> 4) & 0xFFF;
            v2 = v4 & 0xF;
            v1 = v3 & 0xC00;
            if (v2 != 0)
            {
                if (v1 > 0x800)
                {
                    if (v1 == 0xC00)
                    {
                        v7 = BAMSTable[-3077 + v3 + 5];
                        v6 = BAMSTable[-3077 + v3 + 6];
                        return (float)(v7 + (v6 - v7) * v2 * 0.0625);
                    }
                }
                else
                {
                    if (v1 == 0x800)
                    {
                        v7 = -BAMSTable[3072 - v3];
                        v6 = -BAMSTable[3072 - v3 + 1];
                        return (float)(v7 + (v6 - v7) * v2 * 0.0625);
                    }
                    if ((v3 & 0xC00) == 0)
                    {
                        v7 = BAMSTable[BAMSTable.Length - 1 - v3];
                        v6 = BAMSTable[BAMSTable.Length - 1 - v3 - 1];
                        return (float)(v7 + (v6 - v7) * v2 * 0.0625);
                    }
                    if (v1 == 0x400)
                    {
                        v7 = -BAMSTable[-1025 + v3 + 1];
                        v6 = -BAMSTable[-1025 + v3 + 2];
                        return (float)(v7 + (v6 - v7) * v2 * 0.0625);
                    }
                }
                v7 = v8;
                v6 = v8;
                return (float)(v7 + (v6 - v7) * v2 * 0.0625);
            }
            if (v1 > 0x800)
            {
                if (v1 == 0xC00)
                    return BAMSTable[-3077 + v3 + 5];
            }
            else
            {
                if (v1 == 0x800)
                    return -BAMSTable[3072 - v3];
                if ((v3 & 0xC00) == 0)
                    return BAMSTable[BAMSTable.Length - 1 - v3];
                if (v1 == 0x400)
                    return -BAMSTable[-1025 + v3 + 1];
            }
            return v8;
        }
    }
}