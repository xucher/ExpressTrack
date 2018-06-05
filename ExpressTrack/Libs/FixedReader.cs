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

        private Reader reader;
        private Mutex temp = new Mutex();
        public Dictionary<string, TagInfo> tagDic = new Dictionary<string, TagInfo>();

        //  连接天线
        public int ConnectAnt(string antIp) {
            if (antIp == "192.168.0.101" | antIp == "192.168.0.102") {
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
                if (antIp == "192.168.0.101")
                    return 1;
                else return 2;
            }
            if (antIp == "192.168.0.103") {
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

                return 3;
            }

            return 0;
        }

        // 关闭连接
        public void Close() {
            reader.Disconnect();
        }

        // 读取数据
        public int ReadData() {
            temp.WaitOne();
            try {
                TagReadData[] tags = reader.Read(500);
                if (tags.Length != 0) {
                    if (!Char.IsLetter(tags[0].EPCString.Substring(0, 1), 0)) {
                        if (!tags[0].EPCString.StartsWith("4"))
                            AddDic(tags[0]);
                    }

                }
            } catch (ModuleLibrary.OpFaidedException notagexp) {
                if (notagexp.ErrCode == 0x400)
                    return 1;
                else
                    return 2;
            }

            temp.ReleaseMutex();
            return 0;
        }

        //读出的芯片信息填入
        public void AddDic(TagReadData tag) {
            temp.WaitOne();
            string keystr = tag.EMDDataString;
            if (!Char.IsLetter(tag.EPCString.Substring(0, 1), 0)) {
                if (tag != null) {
                    //判断是否是同一件商品
                    if (!tagDic.ContainsKey(keystr)) {
                        TagInfo tempo = new TagInfo {
                            TID = tag.EMDDataString, EPC=tag.EPCString
                        };
                        tagDic.Add(keystr, tempo);
                    }
                }
            }
            temp.ReleaseMutex();
        }
    }
}
