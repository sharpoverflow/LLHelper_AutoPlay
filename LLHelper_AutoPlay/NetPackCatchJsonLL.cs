using SharpPcap;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

public partial class NetPackCatchJsonLL
{

    public delegate void StringDelegate(string str);
    public StringDelegate onNewJsonAdd;
    public StringDelegate onNewHttpAdd;
    public StringDelegate onNewHttpLLAdd;


    //public class JsonResult
    //{
    //    public string json;
    //    public JsonResult(string json)
    //    {
    //        this.name = name;
    //        this.json = json;
    //    }
    //    public override string ToString()
    //    {
    //        if (json.Contains("friend_action_cnt")) return "主页刷新";
    //        if (json.Contains("event_battle_room_id")) return "sm活动房间回应";
    //        if (json.Contains("live_list")) return "歌曲谱面";
    //        return "其他";
    //    }
    //}

    private static Regex reg_http = new Regex(@"HTTP");
    private static Regex reg_json = new Regex(@"/json");
    private static Regex reg_lovelive = new Regex(@"lovelive");
    private int packetId;

    private Dictionary<uint, int[]> packTimeRest = new Dictionary<uint, int[]>();
    private Dictionary<uint, List<PacketWrapper>> packDic = new Dictionary<uint, List<PacketWrapper>>();
    private int packTimeMax = 1000;

    private void OnPacketArrival(object sender, CaptureEventArgs e)
    {
        PacketWrapper packetWrapper = new PacketWrapper(packetId++, e.Packet);

        uint ackn = packetWrapper.tcpPacket.AcknowledgmentNumber;
        if (!packDic.ContainsKey(ackn))
        {
            packDic.Add(ackn, new List<PacketWrapper>());
            packTimeRest.Add(ackn, new int[] { packTimeMax });
        }
        packDic[ackn].Add(packetWrapper);

        List<uint> nd = new List<uint>();
        foreach (var p in packTimeRest)
        {
            packTimeRest[p.Key][0]--;
            if (packTimeRest[p.Key][0] <= 0)
            {
                nd.Add(p.Key);
            }
        }

        foreach (var p in nd)
        {
            packTimeRest.Remove(p);
            packDic.Remove(p);
        }

        //Console.WriteLine("count:" + packTimeRest.Count);

        if (!packetWrapper.tcpPacket.Psh) return;

        byte[] data = CombinePacket(ackn);

        if (data == null) return;

        string s = Encoding.UTF8.GetString(data);

        int index = s.IndexOf("\r\n\r\n");
        if (index < 0) return;
        index += 4;

        string header = s.Substring(0, index);

        if (!HttpCheck(header)) return;

        onNewHttpAdd?.Invoke(s);

        if (!LLHttpCheck(header)) return;

        Console.WriteLine("datalen" + data.Length);

        string body = s.Substring(index);

        if (body.Length < 10) return;

        if(header.IndexOf("POST") == 0)
        {
            int lk = body.IndexOf("{");
            int rk = body.IndexOf("}");
            body = body.Substring(lk, rk - lk + 1);

            string sj = Utils.FormatJsonString(body);
            onNewHttpLLAdd?.Invoke(header + "\r\n\r\n\r\n" + sj);
            onNewJsonAdd?.Invoke(sj);
            return;
        }

        byte[] bodyBytes = null;

        for (int i = 0; i < data.Length - 1; i++)
        {
            if (data[i] == 13 && data[i + 2] == 13 && data[i + 1] == 10 && data[i + 3] == 10)
            {
                i += 4;
                bodyBytes = new byte[data.Length - i];
                for (int t = 0; t < bodyBytes.Length; t++, i++)
                {
                    bodyBytes[t] = data[i];
                }
                break;
            }
        }

        if (bodyBytes == null) return;

        byte[] bodyUncompressedBytes = Utils.Decompress(bodyBytes);

        if (bodyUncompressedBytes == null) return;
        string json = Encoding.UTF8.GetString(bodyUncompressedBytes);

        if (!json.Contains("{")) return;

        json = Utils.Unicode2String(json);
        json = Utils.FormatJsonString(json);


        onNewHttpLLAdd?.Invoke(header + "\r\n\r\n\r\n" + json);
        onNewJsonAdd?.Invoke(json);
    }

    private bool HttpCheck(string s)
    {
        return s.IndexOf("HTTP") == 0 || s.IndexOf("GET") == 0 || s.IndexOf("POST") == 0;
    }

    private bool LLHttpCheck(string s)
    {
        bool result = true;
        try
        {
            //result &= reg_json.Match(s).Success;
            result &= reg_lovelive.Match(s).Success;
            return result;
        }
        catch { }
        return false;
    }

    private byte[] CombinePacket(uint acknowledgmentNumber)
    {
        try
        {
            List<PacketWrapper> pwl = new List<PacketWrapper>(packDic[acknowledgmentNumber].ToArray());
            pwl.Sort(PackSort);
            PackDereplication(pwl);
            if (PackCheck(pwl))
            {
                List<byte> data = new List<byte>();
                for (int i = 0; i < pwl.Count; i++)
                {
                    data.AddRange(pwl[i].tcpPacket.PayloadData);
                }
                if (data.Count < 64)
                {
                    return null;
                }
                return data.ToArray();
            }
        }
        catch { }
        return null;
    }

    private bool PackCheck(List<PacketWrapper> pwl)
    {
        for (int i = 0; i < pwl.Count - 1; i++)
        {
            var tp1 = pwl[i].tcpPacket;
            var tp2 = pwl[i + 1].tcpPacket;

            if (tp2.SequenceNumber != tp1.SequenceNumber + tp1.PayloadData.Length)
            {
                return false;
            }
        }
        return true;
    }

    private void PackDereplication(List<PacketWrapper> pwl)
    {
        if (pwl.Count < 2) return;
        for (int i = pwl.Count - 2; i >= 0; i--)
        {
            var tp1 = pwl[i].tcpPacket;
            var tp2 = pwl[i + 1].tcpPacket;

            if (tp1.SequenceNumber == tp2.SequenceNumber)
            {
                if (tp1.PayloadData.Length >= tp2.PayloadData.Length)
                {
                    pwl.RemoveAt(i + 1);
                }
                else
                {
                    pwl.RemoveAt(i);
                }
            }
        }
    }

    private int PackSort(PacketWrapper x, PacketWrapper y)
    {
        if (x.tcpPacket.SequenceNumber > y.tcpPacket.SequenceNumber)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }






}
