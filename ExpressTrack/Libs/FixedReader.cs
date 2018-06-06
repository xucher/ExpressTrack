using ModuleTech;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ExpressTrack.Libs {
    public class FixedReader {
        // 标签信息
        public class TagInfo {
            public string TID;
            public string EPC;
        }
        public int state = 0;
        public const string ANT1 = "192.168.0.101";
        public const string ANT2 = "192.168.0.102";
        public const string ANT3 = "192.168.0.103";

        private Reader reader;
        private Mutex temp = new Mutex();
        public Dictionary<string, TagInfo> tagDic = new Dictionary<string, TagInfo>();

        // 连接天线
        public void ConnectAnt(string antIp) {
            if (antIp == ANT1 | antIp == ANT2) {
                reader = Reader.Create(antIp, ModuleTech.Region.NA, 1);

                int[] ants = new int[] { 1 };
                SimpleReadPlan searchPlan = new SimpleReadPlan(ants);
                reader.ParamSet("ReadPlan", searchPlan);
                AntPower[] antPower = new AntPower[1];
                for (int i = 0; i < 1; i++) {
                    ushort power = (ushort)((float)3000);
                    antPower[i].AntId = (byte)(i + 1);
                    antPower[i].WritePower = power;
                    antPower[i].ReadPower = power;
                }
                reader.ParamSet("AntPowerConf", antPower);

                EmbededCmdData ecd = new EmbededCmdData(ModuleTech.Gen2.MemBank.TID, 0, 12);
                reader.ParamSet("EmbededCmdOfInventory", ecd);
                if (antIp == ANT1)
                    state = 1;
                else
                    state = 2;
            }
            if (antIp == ANT3) {
                reader = Reader.Create(antIp, ModuleTech.Region.NA, 4);

                int[] ants = new int[] { 3 };//货柜19
                SimpleReadPlan searchPlan = new SimpleReadPlan(ants);
                reader.ParamSet("ReadPlan", searchPlan);
                AntPower[] antPower = new AntPower[4];
                for (int i = 0; i < 4; i++) {
                    ushort power = (ushort)((float)3100);
                    antPower[i].AntId = (byte)(i + 1);
                    antPower[i].WritePower = power;
                    antPower[i].ReadPower = power;
                }
                reader.ParamSet("AntPowerConf", antPower);

                EmbededCmdData ecd = new EmbededCmdData(ModuleTech.Gen2.MemBank.TID, 0, 12);
                reader.ParamSet("EmbededCmdOfInventory", ecd);

                state = 3;
            }
        }

        // 关闭连接
        public void Close() {
            reader.Disconnect();
        }

        public TagInfo ReadData() {
            TagInfo data = null;
            temp.WaitOne();
            try {
                TagReadData[] tags = reader.Read(500);
                if (tags.Length != 0) {
                    data = new TagInfo {
                        TID = tags[0].EMDDataString,
                        EPC = tags[0].EPCString
                    };
                }
            } catch (ModuleLibrary.OpFaidedException e) {
               Console.WriteLine(e.ErrCode);
            }

            temp.ReleaseMutex();
            return data;
        }
    }
}
