//Raw:https://stackoverflow.com/questions/6812068/c-sharp-which-is-the-fastest-way-to-take-a-screen-shot

using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Threading;

public class ScreenStateLogger
{
    private bool isRun;

    public Action<IntPtr, int> onScreenRefreshed;

    public void Start()
    {
        isRun = true;

        Factory1 factory = new Factory1();
        Adapter1 adapter = factory.GetAdapter1(0);
        SharpDX.Direct3D11.Device device = new SharpDX.Direct3D11.Device(adapter);
        Output output = adapter.GetOutput(0);
        Output1 output1 = output.QueryInterface<Output1>();

        int width = output.Description.DesktopBounds.Right;
        int height = output.Description.DesktopBounds.Bottom;

        Texture2DDescription textureDesc = new Texture2DDescription
        {
            CpuAccessFlags = CpuAccessFlags.Read,
            BindFlags = BindFlags.None,
            Format = Format.B8G8R8A8_UNorm,
            Width = width,
            Height = height,
            OptionFlags = ResourceOptionFlags.None,
            MipLevels = 1,
            ArraySize = 1,
            SampleDescription = { Count = 1, Quality = 0 },
            Usage = ResourceUsage.Staging
        };

        Texture2D screenTexture = new Texture2D(device, textureDesc);

        new Thread(() =>
        {

            using (OutputDuplication duplicatedOutput = output1.DuplicateOutput(device))
            {
                while (isRun)
                {
                    try
                    {
                        duplicatedOutput.AcquireNextFrame(5, out OutputDuplicateFrameInformation duplicateFrameInformation, out SharpDX.DXGI.Resource screenResource);
                        using (Texture2D screenTexture2D = screenResource.QueryInterface<Texture2D>())
                        {
                            device.ImmediateContext.CopyResource(screenTexture2D, screenTexture);
                        }
                        DataBox mapSource = device.ImmediateContext.MapSubresource(screenTexture, 0, MapMode.Read, SharpDX.Direct3D11.MapFlags.None);

                        //直接传递图像资源指针,节省处理bitmap的时间
                        onScreenRefreshed?.Invoke(mapSource.DataPointer, width);

                        device.ImmediateContext.UnmapSubresource(screenTexture, 0);

                        screenResource.Dispose();
                        duplicatedOutput.ReleaseFrame();
                    }
                    catch (SharpDXException e)
                    {
                        if (e.ResultCode.Code != SharpDX.DXGI.ResultCode.WaitTimeout.Result.Code)
                        {
                            Console.WriteLine(e.Message);
                            Console.WriteLine(e.StackTrace);
                        }
                    }
                }
            }

        }).Start();
    }

    public void Stop()
    {
        isRun = false;
    }

}
