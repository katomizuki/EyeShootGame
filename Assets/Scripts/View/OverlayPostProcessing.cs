using UnityEngine;
using UnityEngine.Assertions;

namespace View
{
  [RequireComponent(typeof(Camera))]
   internal sealed class OverlayPostProcessing : MonoBehaviour
    {
        [SerializeField] private Color axisColor;
        [SerializeField] private Color sweepColor;
        [SerializeField] private ComputeShader shader;
    
        private const string KernelName = "CSMain";
        private int _kernelHandle;
        private Vector2Int _texSize;
        private Vector2Int _groupSize;
        private Camera _targetCamera;
        private RenderTexture _output;
        private RenderTexture _renderedSource;

        private void CreateTextures()
        {
            Assert.IsNotNull(shader, "shader is null");
            Assert.IsTrue(SystemInfo.supportsComputeShaders, "Not supportsComputeShaders");
            _kernelHandle = shader.FindKernel(KernelName);
            SetupTexSize();
            SetupGroupSize();
            CreateTexture(ref _output);
            CreateTexture(ref _renderedSource);
            SetUpShaderProperties();
        }
        
        private void SetupTexSize()
        {
            // テキスチャサイズをカメラと同じにする
            _targetCamera = GetComponent<Camera>();
            _texSize.x = _targetCamera.pixelWidth;
            _texSize.y = _targetCamera.pixelHeight;   
            shader.SetFloat("texWidth", _texSize.x);
            shader.SetFloat("texHeight", _texSize.y);
            shader.SetFloat("aspectRatio",(float)_texSize.x / (float)_texSize.y);
        }

        private void SetupGroupSize()
        {
            // スレッドグループサイズ取得=>今回8,8,1
            shader.GetKernelThreadGroupSizes(_kernelHandle, out var x, out var y, out _);
            //　スレッドグループ数がx、ｙ方向それぞれどれくらい必要か計算
            _groupSize.x = Mathf.CeilToInt(_texSize.x / (float)x);
            _groupSize.y = Mathf.CeilToInt(_texSize.y / (float)y); 
        }
        
        private void SetUpShaderProperties()
        {
            // Computeshaderにテキスチャをセット
            shader.SetTexture(_kernelHandle, "source", _renderedSource);
            shader.SetTexture(_kernelHandle, "outputTex", _output);
            // 色をセット
            shader.SetVector("axisColor", axisColor);
            shader.SetVector("sweepColor", sweepColor); 
        }

        private void ClearTexture(ref RenderTexture renderTexture)
        {
            renderTexture?.Release();
            renderTexture = null; 
        }

        private void CreateTexture(ref RenderTexture tex)
        {
            tex = new RenderTexture(_texSize.x, _texSize.y, 0);
            tex.enableRandomWrite = true;
            tex.Create(); 
        }

        private void Update()
        {
            // timeを更新してShaderをDispatch
            shader.SetFloat("time", Time.time);
            shader.Dispatch(_kernelHandle, _groupSize.x, _groupSize.y, 1);
        }


        private void OnEnable()
        {
            CreateTextures();
        }

        private void OnDisable()
        {
            ClearTexture(ref _output);
            ClearTexture(ref _renderedSource);
        }

        private void OnDestroy()
        {
            ClearTexture(ref _output);
            ClearTexture(ref _renderedSource);
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Graphics.Blit(source,_renderedSource);
            Graphics.Blit(_output,destination); 
        }
    } 
}
