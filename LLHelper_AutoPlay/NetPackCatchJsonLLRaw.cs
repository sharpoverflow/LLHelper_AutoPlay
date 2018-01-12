using PacketDotNet;
using SharpPcap;
using System;

public partial class NetPackCatchJsonLL
{
    public DeviceListForm deviceListForm;

    private class PacketWrapper
    {
        public int id;
        public TcpPacket tcpPacket;
        public IpPacket ipPacket;
        public PacketWrapper(int id, RawCapture p)
        {
            this.id = id;
            Packet pac = Packet.ParsePacket(p.LinkLayerType, p.Data);
            tcpPacket = pac.Extract(typeof(TcpPacket)) as TcpPacket;
            ipPacket = pac.Extract(typeof(IpPacket)) as IpPacket;
        }
    }

    private PacketArrivalEventHandler arrivalEventHandler;

    private CaptureStoppedEventHandler captureStoppedEventHandler;

    private ICaptureDevice device;

    public void ShowSelectDeviceWindow()
    {
        deviceListForm = new DeviceListForm();
        deviceListForm.Show();
        deviceListForm.OnItemSelected += deviceListForm_OnItemSelected;
    }

    public void Shutdown()
    {
        if (device != null)
        {
            device.StopCapture();
            device.Close();
            device.OnPacketArrival -= arrivalEventHandler;
            device.OnCaptureStopped -= captureStoppedEventHandler;
            device = null;
        }
    }

    private void deviceListForm_OnItemSelected(int itemIndex)
    {
        Console.WriteLine(itemIndex);
        deviceListForm.Hide();
        StartCapture(itemIndex);
    }

    private void StartCapture(int itemIndex)
    {
        packetId = 0;
        device = CaptureDeviceList.Instance[itemIndex];
        arrivalEventHandler = new PacketArrivalEventHandler(OnPacketArrival);
        device.OnPacketArrival += arrivalEventHandler;
        captureStoppedEventHandler = new CaptureStoppedEventHandler(OnCaptureStopped);
        device.OnCaptureStopped += captureStoppedEventHandler;
        device.Open();
        device.Filter = "ip and tcp";
        device.StartCapture();
    }

    private void OnCaptureStopped(object sender, CaptureStoppedEventStatus status)
    {
        if (status != CaptureStoppedEventStatus.CompletedWithoutError)
        {
            Console.WriteLine("Error stopping capture");
        }
    }

}
