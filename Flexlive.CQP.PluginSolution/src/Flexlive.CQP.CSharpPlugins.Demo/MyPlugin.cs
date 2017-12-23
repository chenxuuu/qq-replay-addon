using Flexlive.CQP.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Linq;


namespace Flexlive.CQP.CSharpPlugins.Demo
{
    /// <summary>
    /// 酷Q C#版插件Demo
    /// </summary>
    public class MyPlugin : CQAppAbstract
    {
        private static byte[] result = new byte[4096];
        private static int myProt = 2333;   // 端口  
        static Socket serverSocket;
        
        /// <summary>
        /// 应用初始化，用来初始化应用的基本信息。
        /// </summary>
        public override void Initialize()
        {
            // 此方法用来初始化插件名称、版本、作者、描述等信息，
            // 不要在此添加其它初始化代码，插件初始化请写在Startup方法中。

            this.Name = "接待喵自定义回复插件";
            this.Version = new Version("1.0.0.0");
            this.Author = "晨旭";
            this.Description = "群成员可自定义回复消息";

            IPAddress ip = IPAddress.Parse("127.0.0.1");
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(ip, myProt));  // 绑定 IP 地址：端口  
            serverSocket.Listen(10);    // 设定最多 10 个排队连接请求  
            Console.WriteLine("启动监听 {0} 成功", serverSocket.LocalEndPoint.ToString());
            // 通过 Clientsoket 发送数据  
            Thread myThread = new Thread(ListenClientConnect);
            myThread.Start();

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Enabled = true;
            timer.Interval = 1000;// 执行间隔时间, 单位为毫秒  
            timer.Start();
            timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer1_Elapsed);
        }
        private static int broadcastNew = 0;
        private static string broadcastNewID = "";
        /// <summary>  
        /// 监听客户端连接  
        /// </summary>  
        private static void ListenClientConnect()
        {
            while (true)
            {
                Socket clientSocket = serverSocket.Accept();
                //clientSocket.Send(Encoding.ASCII.GetBytes("Server Say Hello\r\n"));

                Socket myClientSocket = (Socket)clientSocket;
                int receiveNumber;
                receiveNumber = myClientSocket.Receive(result);
                //Console.WriteLine("接收客户端 {0} 消息{1}", myClientSocket.RemoteEndPoint.ToString(), Encoding.UTF8.GetString(result, 0, receiveNumber));
                string replay = Encoding.UTF8.GetString(result, 0, receiveNumber);
                replay = RemoveColorCode(replay);
                if (replay.IndexOf("<") != -1)
                {
                    
                    if (replay.IndexOf("]][[") != -1)
                    {
                        try
                        {
                            string[] str2;
                            str2 = replay.Split(new string[] { "]][[" }, StringSplitOptions.None);
                            foreach (string i in str2)
                            {
                                if( i.IndexOf("<eco100>") == 0)
                                {
                                    long fromQQ = qq_get(i.Replace("<eco100>", ""));
                                    string CoinStr = xml_get(2, fromQQ.ToString());
                                    int CoinsTemp;
                                    if (CoinStr != "")
                                    {
                                        CoinsTemp = int.Parse(CoinStr);
                                    }
                                    else
                                    {
                                        CoinsTemp = 0;
                                    }
                                    CoinsTemp += 100;
                                    SendMinecraftMessage(241464054, CQ.CQCode_At(fromQQ) + "已为玩家" + replay.Replace("<eco100>", "") + "存入100游戏币！");
                                    del(2, fromQQ.ToString());
                                    insert(2, fromQQ.ToString(), CoinsTemp.ToString());
                                }
                                else if (i.IndexOf("<eco500>") == 0)
                                {
                                    long fromQQ = qq_get(i.Replace("<eco500>", ""));
                                    string CoinStr = xml_get(2, fromQQ.ToString());
                                    int CoinsTemp;
                                    if (CoinStr != "")
                                    {
                                        CoinsTemp = int.Parse(CoinStr);
                                    }
                                    else
                                    {
                                        CoinsTemp = 0;
                                    }
                                    CoinsTemp += 500;
                                    SendMinecraftMessage(241464054, CQ.CQCode_At(fromQQ) + "已为玩家" + replay.Replace("<eco500>", "") + "存入500游戏币！");
                                    del(2, fromQQ.ToString());
                                    insert(2, fromQQ.ToString(), CoinsTemp.ToString());
                                }
                                else if (i.IndexOf("<eco1000>") == 0)
                                {
                                    long fromQQ = qq_get(i.Replace("<eco1000>", ""));
                                    string CoinStr = xml_get(2, fromQQ.ToString());
                                    int CoinsTemp;
                                    if (CoinStr != "")
                                    {
                                        CoinsTemp = int.Parse(CoinStr);
                                    }
                                    else
                                    {
                                        CoinsTemp = 0;
                                    }
                                    CoinsTemp += 1000;
                                    SendMinecraftMessage(241464054, CQ.CQCode_At(fromQQ) + "已为玩家" + replay.Replace("<eco1000>", "") + "存入1000游戏币！");
                                    del(2, fromQQ.ToString());
                                    insert(2, fromQQ.ToString(), CoinsTemp.ToString());
                                }
                                else if (i.IndexOf("<eco5000>") == 0)
                                {
                                    long fromQQ = qq_get(i.Replace("<eco5000>", ""));
                                    string CoinStr = xml_get(2, fromQQ.ToString());
                                    int CoinsTemp;
                                    if (CoinStr != "")
                                    {
                                        CoinsTemp = int.Parse(CoinStr);
                                    }
                                    else
                                    {
                                        CoinsTemp = 0;
                                    }
                                    CoinsTemp += 5000;
                                    SendMinecraftMessage(241464054, CQ.CQCode_At(fromQQ) + "已为玩家" + replay.Replace("<eco5000>", "") + "存入5000游戏币！");
                                    del(2, fromQQ.ToString());
                                    insert(2, fromQQ.ToString(), CoinsTemp.ToString());
                                }
                                else if (i.IndexOf("<eco10000>") == 0)
                                {
                                    long fromQQ = qq_get(i.Replace("<eco10000>", ""));
                                    string CoinStr = xml_get(2, fromQQ.ToString());
                                    int CoinsTemp;
                                    if (CoinStr != "")
                                    {
                                        CoinsTemp = int.Parse(CoinStr);
                                    }
                                    else
                                    {
                                        CoinsTemp = 0;
                                    }
                                    CoinsTemp += 10000;
                                    SendMinecraftMessage(241464054, CQ.CQCode_At(fromQQ) + "已为玩家" + replay.Replace("<eco10000>", "") + "存入10000游戏币！");
                                    del(2, fromQQ.ToString());
                                    insert(2, fromQQ.ToString(), CoinsTemp.ToString());
                                }
                                else if (i.IndexOf("<qd>") == 0)
                                {
                                    long fromQQ = qq_get(i.Replace("<qd>", ""));
                                    if(fromQQ==0)
                                        fromQQ = qq_get_unregister(i.Replace("<qd>", ""));
                                    if (fromQQ != 0)
                                    {
                                        if (xml_get(3, fromQQ.ToString()) == System.DateTime.Today.ToString())
                                        {
                                            //SendMinecraftMessage(241464054, CQ.CQCode_At(fromQQ) + "你今天已经签过到啦！");
                                        }
                                        else
                                        {
                                            string last_time = xml_get(3, fromQQ.ToString());
                                            string qdTimesStr = xml_get(7, fromQQ.ToString());
                                            string CoinStr = xml_get(2, fromQQ.ToString());
                                            int CoinsTemp, qdTimesTemp;
                                            if (CoinStr != "")
                                            {
                                                CoinsTemp = int.Parse(CoinStr);
                                            }
                                            else
                                            {
                                                CoinsTemp = 0;
                                            }
                                            if (qdTimesStr != "")
                                            {
                                                qdTimesTemp = int.Parse(qdTimesStr);
                                            }
                                            else
                                            {
                                                qdTimesTemp = 1;
                                            }
                                            if (xml_get(3, fromQQ.ToString()) == System.DateTime.Today.AddDays(-1).ToString())
                                            {
                                                qdTimesTemp++;
                                            }
                                            else
                                            {
                                                qdTimesTemp = 1;
                                            }
                                            Random ran = new Random(System.DateTime.Now.Millisecond);
                                            int RandKey = ran.Next(100, 501);
                                            CoinsTemp += RandKey + qdTimesTemp * 5;
                                            SendMinecraftMessage(241464054, CQ.CQCode_At(fromQQ) + "\r\n签到成功！已连续签到" + qdTimesTemp.ToString() + "天\r\n获得游戏币" + RandKey + "+"+ qdTimesTemp.ToString() + "*5枚！\r\n银行内游戏币" + CoinsTemp + "枚\r\n抽奖次数已重置为五次！\r\n（可使用“五连抽指令”）\r\n回复“帮助”查看如何取钱");
                                            del(2, fromQQ.ToString());
                                            del(3, fromQQ.ToString());
                                            del(4, fromQQ.ToString());
                                            del(7, fromQQ.ToString());
                                            insert(2, fromQQ.ToString(), CoinsTemp.ToString());
                                            insert(7, fromQQ.ToString(), qdTimesTemp.ToString());
                                            insert(3, fromQQ.ToString(), System.DateTime.Today.ToString());
                                            insert(4, fromQQ.ToString(), "0");

                                            string qd_get = xml_get(8, "qd");  //签到数
                                            int qd = 0;
                                            try
                                            {
                                                qd = int.Parse(qd_get);
                                            }
                                            catch { }
                                            qd++;
                                            del(8, "qd");
                                            insert(8, "qd", qd.ToString());
                                        }
                                    }
                                    else
                                    {
                                        //SendMinecraftMessage(241464054, "玩家" + i.Replace("<qd>", "") + "请到群241464054绑定自己的id！");
                                        mcmsg += "|||||command>tm msg " + i.Replace("<qd>", "") + " 请打开sweetcreeper.com并加群！";
                                        broadcastNew = 1;
                                        broadcastNewID = i.Replace("<qd>", "");
                                    }
                                    
                                }
                                else if(i.IndexOf("请打开sweetcreeper") == -1 &&
                                        i.IndexOf("<提示>tm bc 整点发钱") == -1 &&
                                        i.IndexOf("<提示>eco give *") == -1 &&
                                        i.IndexOf("<提示>ban ") == -1 &&
                                        i.IndexOf("<提示>unban ") == -1 &&
                                        i.IndexOf("<提示>kick ") == -1)
                                {
                                    CQ.SendGroupMessage(241464054, i);
                                    ReplayGroupStatic(241464054, i);
                                }
                            }
                        }
                        catch { }
                    }
                    else
                    {
                        if (replay.IndexOf("<eco100>") == 0)
                        {
                            long fromQQ = qq_get(replay.Replace("<eco100>", ""));
                            string CoinStr = xml_get(2, fromQQ.ToString());
                            int CoinsTemp;
                            if (CoinStr != "")
                            {
                                CoinsTemp = int.Parse(CoinStr);
                            }
                            else
                            {
                                CoinsTemp = 0;
                            }
                            CoinsTemp += 100;
                            SendMinecraftMessage(241464054, CQ.CQCode_At(fromQQ) + "已为玩家" + replay.Replace("<eco100>", "") + "存入100游戏币！");
                            del(2, fromQQ.ToString());
                            insert(2, fromQQ.ToString(), CoinsTemp.ToString());
                        }
                        else if (replay.IndexOf("<eco500>") == 0)
                        {
                            long fromQQ = qq_get(replay.Replace("<eco500>", ""));
                            string CoinStr = xml_get(2, fromQQ.ToString());
                            int CoinsTemp;
                            if (CoinStr != "")
                            {
                                CoinsTemp = int.Parse(CoinStr);
                            }
                            else
                            {
                                CoinsTemp = 0;
                            }
                            CoinsTemp += 500;
                            SendMinecraftMessage(241464054, CQ.CQCode_At(fromQQ) + "已为玩家" + replay.Replace("<eco500>", "") + "存入500游戏币！");
                            del(2, fromQQ.ToString());
                            insert(2, fromQQ.ToString(), CoinsTemp.ToString());
                        }
                        else if (replay.IndexOf("<eco1000>") == 0)
                        {
                            long fromQQ = qq_get(replay.Replace("<eco1000>", ""));
                            string CoinStr = xml_get(2, fromQQ.ToString());
                            int CoinsTemp;
                            if (CoinStr != "")
                            {
                                CoinsTemp = int.Parse(CoinStr);
                            }
                            else
                            {
                                CoinsTemp = 0;
                            }
                            CoinsTemp += 1000;
                            SendMinecraftMessage(241464054, CQ.CQCode_At(fromQQ) + "已为玩家" + replay.Replace("<eco1000>", "") + "存入1000游戏币！");
                            del(2, fromQQ.ToString());
                            insert(2, fromQQ.ToString(), CoinsTemp.ToString());
                        }
                        else if (replay.IndexOf("<eco5000>") == 0)
                        {
                            long fromQQ = qq_get(replay.Replace("<eco5000>", ""));
                            string CoinStr = xml_get(2, fromQQ.ToString());
                            int CoinsTemp;
                            if (CoinStr != "")
                            {
                                CoinsTemp = int.Parse(CoinStr);
                            }
                            else
                            {
                                CoinsTemp = 0;
                            }
                            CoinsTemp += 5000;
                            SendMinecraftMessage(241464054, CQ.CQCode_At(fromQQ) + "已为玩家" + replay.Replace("<eco5000>", "") + "存入5000游戏币！");
                            del(2, fromQQ.ToString());
                            insert(2, fromQQ.ToString(), CoinsTemp.ToString());
                        }
                        else if (replay.IndexOf("<eco10000>") == 0)
                        {
                            long fromQQ = qq_get(replay.Replace("<eco10000>", ""));
                            string CoinStr = xml_get(2, fromQQ.ToString());
                            int CoinsTemp;
                            if (CoinStr != "")
                            {
                                CoinsTemp = int.Parse(CoinStr);
                            }
                            else
                            {
                                CoinsTemp = 0;
                            }
                            CoinsTemp += 10000;
                            SendMinecraftMessage(241464054, CQ.CQCode_At(fromQQ) + "已为玩家" + replay.Replace("<eco10000>", "") + "存入10000游戏币！");
                            del(2, fromQQ.ToString());
                            insert(2, fromQQ.ToString(), CoinsTemp.ToString());
                        }
                        else if (replay.IndexOf("请打开sweetcreeper") == -1 && 
                                 replay.IndexOf("<提示>tm bc 整点发钱") == -1 &&
                                 replay.IndexOf("<提示>eco give *") == -1 &&
                                 replay.IndexOf("<提示>ban ") == -1 &&
                                 replay.IndexOf("<提示>unban ") == -1 &&
                                 replay.IndexOf("<提示>kick ") == -1)
                        {
                            CQ.SendGroupMessage(241464054, replay);
                            ReplayGroupStatic(241464054, replay);
                        }
                    }
                }
                clientSocket.Send(Encoding.UTF8.GetBytes("ok233"));
                mcmsg = mcmsg.Replace("\r\n", "|||||<");
                mcmsg = mcmsg.Replace("[CQ:image,file=", "[图片：");
                mcmsg = mcmsg.Replace("[CQ:at,qq=", "[@");
                mcmsg = mcmsg.Replace("[CQ:record,file=", "[语音：");

                clientSocket.Send(Encoding.UTF8.GetBytes(mcmsg));
                mcmsg = "";

                myClientSocket.Shutdown(SocketShutdown.Both);
                myClientSocket.Close();
            }//与minecraft消息进行通讯
        }

        public static string mcmsg = "";

        private static string[] GoodThings= {
                                            "补番：现在不补，更待何时？",
                                            "逛B站：各种涨姿势",
                                            "抽卡开箱子：哇都是宝贝",
                                            "早起：新的一天干劲满满",
                                            "时间众筹：移动速度变快",
                                            "啪啪啪：啪啪啪啪啪",
                                            "吃鸡：大吉大利，晚上吃鸡！",
                                            "调戏群主：群主害羞地发了张女装照",
                                            "抚摸喵星人：喵？",
                                            "追番：更新好及时啊",
                                            "看书：知识是人类进步的阶梯",
                                            "下馆子：今天餐馆优惠啦",
                                            "看电视：都是喜欢的节目",
                                            "告白：被FFF烧死",
                                            "戴帽子：白色象征权力，绿色代表力量",
                                            "逛H站：硬盘要满了啦",
                                            "立FLAG：做事要靠自己的努力了啦",
                                            "吃馒头：吃米饭是异端",
                                            "出去玩：一天好心情",
                                            "带上伞：没准会下雨",
                                            "逛知乎：各种涨姿势，轮带逛",
                                            "敲代码：虽然不知道写了什么，但是总觉得一定没有bug",
                                            "网购：好多便宜的东西",
                                            "早睡：今晚没有人陪你哦",
                                            "听音乐：🎵~",
                                            "迷の空缺位"};
        private static string[] BadThings = {
                                            "补番：各种被剧透",
                                            "逛B站：遇见小学生撕逼",
                                            "抽卡开箱子：都是些什么鬼",
                                            "早起：一整天都想睡",
                                            "时间众筹：-1year",
                                            "啪啪啪：会卡在里面",
                                            "吃鸡：以掉下来就被打死",
                                            "调戏群主：被禁言一个月",
                                            "抚摸喵星人：一嘴毛",
                                            "追番：怎么还没更新",
                                            "看书：看不懂",
                                            "下馆子：又浪费钱出去吃",
                                            "看电视：这节目好假啊",
                                            "告白：好人卡get√",
                                            "戴帽子：被嘲笑",
                                            "逛H站：都是重口味",
                                            "立FLAG：你一定会成功",
                                            "吃馒头：米饭才是王道",
                                            "出去玩：共享单车全都是坏的",
                                            "带上伞：没下雨，被嘲笑",
                                            "逛知乎：知乎小管家把你封了",
                                            "敲代码：知道写了什么，但是就是找不出bug的原因",
                                            "网购：会被坑",
                                            "早睡：会在半夜醒来，然后失眠",
                                            "听音乐：软件卡主，文件丢失",
                                            "迷の空缺位"};
        private static string[] itsays = {
            "1000-7",
            "70万都没有你还想当吧主",
            "「……這個世界就是變態的。」",
            "「人生在世数十年，小事不要太计较！」",
            "啊哈哈，佐祐理不知道",
            "爱的供养，再问自杀",
            "矮穷矬",
            "哎呀我摔倒了，要亲亲才能起来",
            "暗中观察",
            "傲娇毁一生",
            "奥特oo",
            "八点二十发",
            "霸气侧漏",
            "把我血加住，我有盾墙",
            "白富美",
            "百合无限好，可惜生不了",
            "报复社会",
            "保证打不死你",
            "脖子以下到脚背以上形成了一个凄惨的平面",
            "背后怕是有肮脏的屁眼交易",
            "笨蛋是不会感冒的",
            "本站立足于美利坚合众国，对全球华人服务，受北美法律保护",
            "别人家的oo",
            "别说了，我",
            "不被发现就不算犯罪哦",
            "不得了",
            "不服来辩",
            "不就是一块石头么，看我用高达把它推回去",
            "不明真相的ooxx",
            "不能描写的方式",
            "不能同意更多",
            "不抛弃不放弃",
            "不如跳舞",
            "不是敌人太强大，只是队友太oo",
            "不是很懂你们oo",
            "不是萝莉控，是女权主义者",
            "不是oo，是x！",
            "不要扶他",
            "不要和我说什么oo",
            "不要靠近我啊",
            "不要问我oo是谁",
            "不要在意细节",
            "不作死就不会死，为什么不明白！",
            "不oo不舒服斯基",
            "不oo的话不就只能去死了吗",
            "操到叫苦连天",
            "厕纸",
            "蝉在叫人坏掉",
            "超高校级的oo",
            "车塞卡",
            "沉迷学习，日渐消瘦",
            "诚哥死得早",
            "吃饭睡觉打oo",
            "吃我龙神之剑",
            "穿山甲到底说了什么",
            "纯爷们从不回头看爆炸",
            "此处应该有掌声",
            "此生无悔入东方，来世愿生幻想乡",
            "从小o到大",
            "从业二十年，做人做事，无愧于心",
            "大葱插菊花治疗感冒",
            "大力出奇迹",
            "打麻将真开心啊",
            "大清亡了",
            "打土豪，分手办",
            "大丈夫萌大奶",
            "带球撞人",
            "但是我拒绝",
            "单纵就是干",
            "当你拥有ooo",
            "当然是选择原谅她",
            "今天，一个死宅实现了他的梦想",
            "德国的科学技术是世界第一",
            "地球君又杯具了",
            "敌羞吾去脱她衣",
            "东京电视台最强传说",
            "东吴爱萝莉，曹魏控人妻，蜀汉全是基",
            "都是oo的错",
            "对方不想和你说话并向你扔了oo",
            "多大仇",
            "堕落！萌死他卡多！",
            "二货，快来条士力架",
            "而且没有人爱你",
            "而是你要享受这个过程",
            "Excuse me???",
            "发图不发种，菊花万人捅",
            "反射弧太长",
            "放铳输一局，缩卵输一生",
            "放开那个oo",
            "飞龙骑脸怎么输",
            "粉红的切开来里面都是黑的",
            "风险投资",
            "弗拉格综合征",
            "父母双亡，有妹有房",
            "嘎嘣脆鸡肉味",
            "该来的总会来的",
            "感觉亏了一个亿",
            "干了这碗恒河水",
            "刚刚o了一下",
            "搞事情",
            "告诉oo我还爱她",
            "高阻绿",
            "各个都是人才，说话又好听，超喜欢在里面",
            "哥哥让开这样我杀不了那家伙",
            "给您拜年了",
            "给dalao递oo",
            "根本停不下来",
            "根据相关法律法规和政策，部分搜索结果未予显示",
            "挂科比不挂柯南",
            "官方逼死同人",
            "官方吐槽",
            "广州宅报",
            "国家欠我一个oo",
            "过去的oo弱爆了",
            "国之将亡，遍地舰娘",
            "害怕",
            "还是种田比较适合老子",
            "还有马桶盖子",
            "好多孩子看到这个根本把持不住",
            "好平如潮",
            "好气啊，但还是要保持微笑",
            "好讨厌，眼泪一直停不下来",
            "毫无ps痕迹",
            "和怪兽战斗是自卫队的传统",
            "喝最烈的酒",
            "嘿嘿……怎麼說呢，說起來有點下流…我竟然…勃起了",
            "很了不起的大象先生吧",
            "恨妹不成穹",
            "画个圈圈诅咒你",
            "画个xx硬说是oo",
            "皇上，臣妾做不到啊",
            "回老家结婚",
            "活久见",
            "Interesting",
            "基本国情",
            "记得关好门",
            "计划通",
            "技术宅拯救世界",
            "简直就是狂欢一样",
            "讲道理",
            "教练，我想ooo",
            "脚只是装饰而已，上面的大人物是不会懂的",
            "揭棺而起 起驾回棺",
            "进击的oo",
            "晋太元中",
            "今天的风儿好喧嚣啊",
            "警察叔叔，就是这个人",
            "净TM扯淡",
            "就打德",
            "AF可能缺少淡水",
            "舅舅党",
            "就算不结婚有妹妹不就好了吗",
            "就算是神也杀给你看",
            "就算是oo，只要有爱就没问题",
            "就这样插进去就行了",
            "巨乳只有下垂的未来",
            "绝望了，我对这个oo的世界绝望了",
            "君日本语本当上手",
            "卡在了奇怪的地方",
            "开局一把刀装备全靠捡",
            "开始你的表演",
            "看啊，你的死兆星在天上闪耀",
            "看见oo我就进来了",
            "看来你不懂生命的可贵",
            "砍了那只鸭",
            "可爱的oo早已",
            "可爱即正义",
            "克里斯关下门",
            "可是那一天，我有了新的想法",
            "可喜可贺",
            "可惜是个oo",
            "可以的，ooo",
            "空降",
            "恐怕罪魁祸首就是oo",
            "空手拆高达",
            "Konmai quality",
            "快拔网线!",
            "快到碗里来",
            "快看他画风和我们不一样",
            "快闪开我按错了",
            "来盘昆特牌吧",
            "来相思树下",
            "Lame Uncomfortable Laugh",
            "蓝瘦，香菇",
            "老子有oo你怕不怕",
            "理都懂",
            "厉害了我的哥",
            "历史总是惊人地相似",
            "联邦的MS都是怪物吗",
            "连我爸爸都没有打过我",
            "连自己心爱的人都救不了，我还算什么oo",
            "楼主好人一生平安",
            "论oo的xxx",
            "萝莉有三好，身娇腰柔易推倒",
            "妈的智障",
            "妈妈再也不用担心我的oo",
            "Mad in chine",
            "满状态原地复活",
            "冒险家翻抽屉有什么不对",
            "美国其实并不存在",
            "没毛病",
            "没图你说个oo",
            "萌娘百科收录希望",
            "萌音绕梁，三日不绝",
            "谜已经全部解开了",
            "明天来oo上班",
            "魔法少女oo",
            "目睹事件全过程的O先生",
            "木有鱼丸",
            "哪里不会点哪里",
            "那万一赢了呢",
            "那一天，人类终于想起了oo的恐怖",
            "奈何共军有高达",
            "奶奶曾经说过",
            "男人变态有什么错",
            "脑装屎",
            "你暴露时间了",
            "你不会百度吗",
            "你才oo，你们全家都oo",
            "你从什么时候开始产生了oooo的错觉",
            "你的爱还不够啊",
            "你的好友oo已上线",
            "你的英勇长存人心",
            "你还记得自己是男的",
            "你经历过绝望吗",
            "你看看你",
            "你渴望力量吗",
            "你妈逼你结婚了吗",
            "你妈喊你回家吃饭",
            "你们……有没有oo",
            "你们城里人真会玩",
            "你们的关系可真好啊",
            "你们都是坏人",
            "你们都是我的翅膀",
            "你们对力量一无所知",
            "你们感受一下",
            "你们尽管oo，xx了算我输",
            "你们两个，干脆交往算啦",
            "你们这是自寻死路",
            "你傻逼！再见！",
            "你是猴子请来的救兵吗",
            "你谁啊",
            "你为何这么屌",
            "你为什么不问问神奇海螺呢",
            "你为什么这么叼",
            "你行你上啊",
            "你胸太小不要说话",
            "你已经死了",
            "你咋不上天呢",
            "你在大声什么啦",
            "你这么厉害怎么不去青青草原抓羊",
            "你这么oo，你xx知道吗",
            "你知道的太多了",
            "你制杖吗？不，我贩剑。",
            "酿了你哦",
            "你tm感动了我",
            "牛顿死得早",
            "浓浓的超好喝",
            "O成狗",
            "O分钟惨案",
            "O军马鹿",
            "O月是xx的oo",
            "O在起跑线",
            "Oo is watching you",
            "Oo，你算计我！oo！",
            "Oo，我只服xx",
            "Oo必须死",
            "Oo大法好",
            "Oo的人生不需要解释",
            "Oo的笑容由我来守护",
            "Oo点击就送",
            "Oo多如狗，xx满地走",
            "Oo儿女多奇志",
            "Oo还能再战xx年",
            "Oo还在，xx没了",
            "Oo还只是个孩子",
            "Oo好可爱啊oo",
            "Oo黑丧心病狂",
            "Oo很萌的，你们不要黑她",
            "Oo救不了xx",
            "Oo可绕地球x圈",
            "Oo哭晕在厕所",
            "Oo了，我不做xx的粉丝了",
            "Oo美如画",
            "Oo你好，oo再见",
            "Oo你突破盲點了",
            "Oo让你滚，你就不得不滚",
            "Oo人民发来贺电",
            "Oo如同大小姐的威严一样伟岸",
            "Oo什么的最讨厌了",
            "Oo是本体",
            "Oo是检验xx的唯一标准",
            "Oo是什么，能吃吗",
            "Oo是完全没有必要的存在",
            "Oo是一种思想，而思想是不怕xx的",
            "Oo望着自己贫瘠的胸部说道",
            "Oo我老婆",
            "Oo五分钟，xx两小时",
            "Oo已经阻止不了我了",
            "Oo已死",
            "Oo有毒",
            "Oo又死了真没人性",
            "Oo与xx之间的惨烈修罗场",
            "Oo在下很大的一盘棋",
            "Oo真是天使",
            "Oo只能帮你到这儿了",
            "Oo只是个oo",
            "Ooo，我们走",
            "Oooo，av10492",
            "配合oo食用风味更佳",
            "贫乳是稀有价值",
            "普通oo，文艺oo和2boo",
            "其实我是你爸爸",
            "其他人做得到吗",
            "前方高能反应",
            "前方oo福利",
            "前略，天国的oo",
            "千万不要搜索oo",
            "枪毙十分钟",
            "抢铁集团",
            "亲妹妹不如干妹妹",
            "请允许我做一个悲伤的表情",
            "去死两次",
            "然而并没有什么卵用",
            "然而oo早就看穿了一切",
            "饶罗翔教你截图",
            "人被杀就会死",
            "人家拿小拳拳捶你胸口",
            "人类才勇敢",
            "人类已经失去了oo的权利了",
            "人数过万，智商减半",
            "任性可是女孩子的天性呢",
            "认真你就输了",
            "日日夜夜",
            "如果你是oo你还笑得出来吗",
            "如果你ooo我们还是朋友",
            "如果奇迹有颜色，那一定是橙色",
            "如果oo就是神作了",
            "如今又再度变得温顺了",
            "萨满，又疯一个",
            "三个人结婚吧",
            "三秒规则",
            "三年起步，最高死刑",
            "三索必须死",
            "三天三夜",
            "上交给国家",
            "少女祈祷中",
            "烧死那对异性恋",
            "射爆",
            "社会我oo",
            "摄影师你明天不用来上班了",
            "神马都是浮云",
            "神说你还不能死在这里",
            "圣光啊，你有看到那个oo吗",
            "声优不要钱",
            "声优都是怪物",
            "声优怒领工资",
            "时代在进步",
            "世界上没有什么事是一oxx不能解决的，如果有，那就两o",
            "视觉模糊",
            "十五字",
            "是在下输了",
            "是oo的friends呢",
            "收了可观的小费后，酒馆老板小声道",
            "输出基本靠吼",
            "兽娘动物园:属性",
            "帅不过三秒",
            "谁过来谁先死",
            "谁指使你来的",
            "瞬间爆炸",
            "说鸡不说吧，文明你我他",
            "四舍五入就是一个亿",
            "死宅真恶心",
            "酸甜苦辣咸，ooooo",
            "虽不明，但觉厉",
            "虽然我可爱又迷人，但我会招来死亡",
            "虽然我谦虚又宽容，但是有三种人我无法忍受",
            "虽现狗",
            "所累哇多卡纳",
            "所罗门哟，我又回来了",
            "太太我喜欢你啊",
            "躺着也中枪",
            "天降软妹",
            "甜食是装在另一个胃里的",
            "天下漫友是一家",
            "天诛八尺，还我公图",
            "土豪我们做朋友好不好",
            "图样图森破",
            "挖掘机技术哪家强",
            "万物皆可萌",
            "万物皆虚，万事皆允",
            "未被穿过的胖次是没有价值的",
            "为了保护我们心爱的ooo……成为偶像！",
            "为什么放弃治疗",
            "为什么没有oo？",
            "为什么要伤害我们的脖子",
            "为什么要说又呢",
            "为王的诞生，献上礼炮！",
            "我，秦始皇，打钱",
            "我不是 我没有",
            "我不需要性爱",
            "我不要看繁星",
            "我不做人了",
            "我曾经把你当做过自慰对象",
            "我从未见过有如此厚颜无耻之人",
            "我的4S笑而不语",
            "我的车里有空调",
            "我的妹妹不可能这么可爱",
            "我的内心毫无波动，甚至还想笑",
            "我的字典里没有oo",
            "我的oo已经饥渴难耐了",
            "我对普通的oo没有兴趣",
            "我给你一次重新组织语言的机会",
            "我跟你讲，ooㄋㄟㄋㄟ，赞",
            "我还没出力，你就倒下了",
            "我好兴奋呀",
            "我和你将在那个新世界里成为新的夏娃和夏娃",
            "我和我的小伙伴们都惊呆了",
            "我怀疑你是oo",
            "我会到处乱说吗",
            "我今天吃药的时候看到一个新闻",
            "我敬你是条汉子",
            "我就是高达",
            "我就是叫紫妈怎么了",
            "我居然在oo上看xx",
            "我觉得我还能再抢救一下",
            "我觉得我优势很大",
            "我开始方了",
            "我看过报道，这个人后来死了",
            "我考虑了一下还是无法接受啊",
            "我可能oo了假的xx",
            "我裤子都脱了你就让我看这个",
            "我们不用很麻烦很累就可以成佛",
            "我们成年人看了都感觉不适",
            "我们的同志遍布五湖四海，甚至打入了某些组织的内部",
            "我们的宇宙充满了质子，中子，电子，还有奶子",
            "我们的征途是星辰大海",
            "我们来削弱oo吧",
            "我们中出了叛徒",
            "我能吞下玻璃而不伤身体",
            "我平胸我骄傲，我为国家省布料",
            "我凭自己本事oo，为什么要xx",
            "我去年买了个表",
            "我认为H是不对的",
            "我日你先人",
            "我上电视了",
            "我是输给了地球的重力",
            "我听了顿时怒火就上来了",
            "我为自己代言",
            "我下面给你吃",
            "我现在内心感到悲痛欲绝",
            "我想静静",
            "我想起来了",
            "我选择死亡",
            "我要成为新世界的卡密",
            "我已经看到结局了",
            "我有一个大胆的想法",
            "我有异议",
            "我有知识我自豪",
            "我真是个罪孽深重的女人",
            "我整个人都oo了",
            "我只是路过的○○",
            "我ooo就算饿死不会吃你一点东西",
            "无敌的oo倒下了",
            "无敌是多么寂寞",
            "无法直视",
            "污姬白凤丸",
            "吾心吾行澄如明镜，所作所为皆为正义",
            "吓得我都oo了",
            "下个ID见",
            "羡慕嫉妒恨",
            "现实就是个垃圾游戏",
            "现已加入肯德基豪华午餐",
            "咸鱼王，我当定了",
            "现在，我的手中抓住了未来",
            "现在是你比较强",
            "想不到你居然是这种oo",
            "乡队员不是在与杰克的搏斗中死了吗",
            "向黑恶势力低头",
            "想死一次吗",
            "相信我，你也可以变成光",
            "小火把在闲逛",
            "小鸟游家要小心一个叫x太的男人",
            "小学生真是太棒了",
            "写作oo读作xx",
            "心如苍井空似水",
            "信息量很大",
            "性别不同怎么谈恋爱",
            "胸不平何以平天下，乳不巨何以聚人心",
            "胸部什么的，明明只是装饰",
            "胸刹现场",
            "玄不救非，氪不改命",
            "亚瑟王不懂人心",
            "眼睛，我的眼睛",
            "眼睛里进了oo",
            "羊角地铁",
            "要上了ooo——xx的储备足够吗",
            "要优雅，不要污",
            "也是蛮拼的",
            "一把剑就直接穿过去了",
            "一本正经地胡说八道",
            "一大波僵尸正在接近",
            "一大团广场",
            "一代补丁一代神",
            "一旦接受了这种设定",
            "一定是我打开的方式不对",
            "一对百合一对基，剩下一个是苦逼",
            "一飞冲天啊,我!",
            "一股恋爱喜剧的酸臭味",
            "一家人最重要的是整整齐齐",
            "已经吃不下了",
            "已经没什么好害怕的了",
            "已经没有人为我站出来说话，因为他们都被抓走了",
            "一脸懵逼",
            "以抢钱为宗旨，待旅客如孙子",
            "一曲忠诚的赞歌",
            "一入oo深似海，从此节操是路人",
            "艺术就是爆炸",
            "以为是oo吗？残念！是xx！",
            "以下省略oo字",
            "以牙还牙，加倍奉还",
            "一言不合就oo",
            "一直被oo，从未被超越",
            "因为很重要所以说了两遍",
            "因为神圣的F2A链接着我们每一个人",
            "因为我是oo",
            "樱trick综合征",
            "勇士喜欢巨乳有什么错",
            "用我一生节操换ooo永不完结",
            "又到了交配的季节",
            "有个能干的妹妹真好",
            "又黑我大oo",
            "有家，有爱，有欧派",
            "有件事说出来你可能不信",
            "友军之围",
            "有奇怪的东西混进去了",
            "有什么东西要觉醒了",
            "有一种杀了他就能升级的感觉",
            "远的不是距离，而是次元啊",
            "元芳你怎么看",
            "远看炮塔吓死人，近看五对负重轮",
            "愿天下有情人都是失散多年的亲兄妹",
            "在我眼里都是渣",
            "暂停学表情",
            "怎么办，我也很绝望啊",
            "宅男费纸，宅女费电",
            "战斗力膨胀",
            "战斗力只有5的渣渣",
            "战个痛快",
            "涨姿势",
            "这不科学",
            "这点题我给满分",
            "这个锅我不背",
            "这个男人，有两把刷子",
            "这个年纪的男孩子对胸部感兴趣不是理所当然的吗",
            "这个时候应该@oo",
            "这个oo是会玩儿的",
            "这火我不传",
            "这就是外国人少的原因",
            "这么可爱一定是男孩子",
            "这破o吃枣药丸",
            "这是何等的灵压啊",
            "这是计划的一部分",
            "这是禁止事项",
            "这是LoveLive",
            "这是人干的事吗",
            "这是我爸爸在夏威夷教我的",
            "这是我最后的波纹",
            "这是一道送分题",
            "这是一个圈套",
            "这是一个神奇的oo",
            "这是一个oo的故事",
            "这时只要微笑就可以了",
            "这首xxx送给oo",
            "这虽然是游戏，但可不是闹着玩的",
            "这腿我能玩一年",
            "这游戏能玩？",
            "这种全靠运气的垃圾游戏",
            "侦测到在途的聚变打击",
            "真当自己大小姐",
            "真当oo不上xx",
            "真是肤浅",
            "真是HIGH到不行",
            "真相并非永远只有一个",
            "真相永远只有一个",
            "正面上我啊",
            "之后干了个爽",
            "只要出问题C4都能搞定",
            "只要打不中就没有什么大不了",
            "只要可爱就算是男孩子也没关系",
            "只要杀了oo我随便你搞",
            "只有oo知道的世界",
            "主角胜于嘴炮，反派死于话多",
            "诸君，我喜欢oo",
            "抓住一只野生的oo",
            "专注oo三十年",
            "自带BGM",
            "自带oo",
            "自古二楼出oo",
            "自古红蓝出CP",
            "自古蓝毛多苦逼",
            "自古OP多剧透",
            "自古肉番出燃曲",
            "自挂东南枝",
            "梓喵打酱油",
            "字幕组调皮了",
            "总有刁民想害朕",
            "总有一天你们会看着我画的东西撸",
            "总有一天我的生命将抵达终点，而你，将加冕为王",
            "走错片场了",
            "钻头是男人的浪漫",
            "最强的ooo一切都是必然的",
            "最上川"
        };  //613句

        public static int isOpenScan = 0;
        public static int scanCount = 0;

        private void Timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)  //定时程序
        {

            // 得到 hour minute second  如果等于某个值就开始执行某个程序。  
            int intHour = e.SignalTime.Hour;
            int intMinute = e.SignalTime.Minute;
            int intSecond = e.SignalTime.Second;
            // 定制时间
            //int iHour = 10;
            int iMinute = 00;
            int iSecond = 00;

            // 设置每个整点开始执行  
            if (intMinute == iMinute && intSecond == iSecond)
            {
                //SendMinecraftMessage(241464054, "整点发钱！");
                Random ran = new Random(System.DateTime.Now.Millisecond);
                mcmsg += "|||||command>tm bc 整点发钱！";
                mcmsg += "|||||command>eco give * " + ran.Next(0, 200);
            }

            // 十二点执行
            if (intMinute == 0 && intSecond == 10 && intHour == 0)
            {
                string qd_get = xml_get(8, "qd");
                int qd = 0;
                try
                {
                    qd = int.Parse(qd_get);
                }
                catch { }
                del(8, "qd");
                Random ran = new Random();
                SendMinecraftMessage(241464054, "新的一天已经到来了哦，现在时间是\r\n" + DateTime.Now.ToString() + "\r\n昨日一共有" + qd + "人签到哦\r\n" + "今日吉言：" + itsays[ran.Next(0, 613)]);
            }

            //if (broadcastNew%3== 0)
            //{
            //    mcmsg += "|||||command>say " + broadcastNewID + " 请打开sweetcreeper.com并加群！";
            //    broadcastNew++;
            //}
            //if(broadcastNew == 20)
            //{
            //    mcmsg += "|||||command>kick " + broadcastNewID + " 请打开sweetcreeper.com并加群！";
            //    broadcastNew = 0;
            //}

        }


        /*
        public void theout(object source, System.Timers.ElapsedEventArgs e)
        {
            if (isOpenScan == 1)
            {
                if (HttpGet("http://wfstc.hpu.edu.cn/list.aspx?id=56", "").IndexOf("第十五周考试监考安排") >= 0)
                {
                    CQ.SendPrivateMessage(961726194, "第十五周考试监考安排已发布，请尽快查看：http://wfstc.hpu.edu.cn/list.aspx?id=56\r\n请手动停止任务");
                    CQ.SendPrivateMessage(1371773817, "第十五周考试监考安排已发布，请尽快查看：http://wfstc.hpu.edu.cn/list.aspx?id=56\r\n请手动停止任务");
                    //isOpenScan = 0;
                }
                //else
                //{
                //    scanCount++;
                //    CQ.SendPrivateMessage(961726194, "第十四周考试监考安排还没发布，任务已执行" + scanCount + "次。");
                //    CQ.SendPrivateMessage(1371773817, "第十四周考试监考安排还没发布，任务已执行" + scanCount + "次。");
                //}
            }
        }*/

        /// <summary>
        /// 应用启动，完成插件线程、全局变量等自身运行所必须的初始化工作。
        /// </summary>
        public override void Startup()
        {
            //完成插件线程、全局变量等自身运行所必须的初始化工作。
            //CQ.SendPrivateMessage(961726194, "机器人已启动，当前第十五周考试监考安排扫描任务已关闭，输入“启动”启动扫描，发送“查看”获取实时发布情况");
            //CQ.SendPrivateMessage(1371773817, "机器人已启动，当前第十五周考试监考安排扫描任务已关闭，输入“启动”启动扫描，发送“查看”获取实时发布情况");
        }

        /// <summary>
        /// 打开设置窗口。
        /// </summary>
        public override void OpenSettingForm()
        {
            // 打开设置窗口的相关代码。
            FormSettings frm = new FormSettings();
            frm.ShowDialog();
        }

        
        /// <summary>
        /// Type=21 私聊消息。
        /// </summary>
        /// <param name="subType">子类型，11/来自好友 1/来自在线状态 2/来自群 3/来自讨论组。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="msg">消息内容。</param>
        /// <param name="font">字体。</param>
        public override void PrivateMessage(int subType, int sendTime, long fromQQ, string msg, int font)
        {
            // 处理私聊消息。
            //CQ.SendPrivateMessage(fromQQ, String.Format("[{0}]你发的私聊消息是：{1}", CQ.ProxyType, msg));
            if (msg.ToUpper() == "赞我")
            {
                CQ.SendPraise(fromQQ, 10);
                CQ.SendPrivateMessage(fromQQ, "已为你一次性点赞十次，每天只能十次哦");
                //CQ.SendGroupMessage(fromGroup, "已为QQ" + fromQQ + "点赞十次");
            }
            else if (msg == "pixel")
            {
                int picCount;
                try
                {
                    picCount = int.Parse(xml_get(20, "count"));
                }
                catch
                {
                    CQ.SendPrivateMessage(fromQQ, "遇到致命性错误，请联系晨旭修复");
                    return;
                }
                CQ.SendPrivateMessage(fromQQ, "[CQ:image,file=pixel_game\\" + picCount + ".png]\r\n当前图片已被修改过" + picCount + "次。");
            }
            else if (msg.IndexOf("pixel") == 0 && (msg.Length - msg.Replace("/", "").Length) == 2)
            {
                string fromqqtime_get = xml_get(20, fromQQ.ToString());
                int fromqqtime = 0;
                try
                {
                    fromqqtime = int.Parse(fromqqtime_get);
                }
                catch { }

                if (ConvertDateTimeInt(DateTime.Now) - fromqqtime < 60)
                {
                    CQ.SendPrivateMessage(fromQQ, "你需要再等" + (60 - ConvertDateTimeInt(DateTime.Now) + fromqqtime) + "秒才能继续放像素点");
                    return;
                }

                string get_msg = msg.Replace("pixel", ""), getx = "", gety = "", getcolor = "";
                int placex, placey;

                string[] str2;
                int count_temp = 0;
                str2 = get_msg.Split('/');
                foreach (string i in str2)
                {
                    if (count_temp == 0)
                    {
                        getx = i.ToString();
                        count_temp++;
                    }
                    else if (count_temp == 1)
                    {
                        gety = i.ToString();
                        count_temp++;
                    }
                    else if (count_temp == 2)
                    {
                        getcolor = i.ToString();
                        count_temp++;
                    }
                }
                try
                {
                    placex = int.Parse(getx) - 1;
                    placey = int.Parse(gety) - 1;
                    if (getcolor.IndexOf("#") == -1 || placex > 99 || placey > 99)
                        throw new ArgumentNullException("fuck wrong color");
                }
                catch
                {
                    CQ.SendPrivateMessage(fromQQ, "放置像素点时遇到未知错误，请检查颜色与坐标是否正确");
                    return;
                }
                int picCount;
                try
                {
                    picCount = int.Parse(xml_get(20, "count"));
                }
                catch
                {
                    CQ.SendPrivateMessage(fromQQ, "遇到致命性错误，请联系晨旭修复");
                    return;
                }

                try
                {
                    string picPath = @"C:\Users\Administrator\Desktop\kuqpro\data\image\pixel_game\" + picCount + ".png";
                    Bitmap pic = ReadImageFile(picPath);
                    pic = SetPoint(pic, ColorTranslator.FromHtml(getcolor), placex, placey);

                    picCount++;
                    picPath = @"C:\Users\Administrator\Desktop\kuqpro\data\image\pixel_game\" + picCount + ".png";
                    pic.Save(picPath);
                }
                catch
                {
                    CQ.SendPrivateMessage(fromQQ, "遭遇未知错误");
                    return;
                }


                del(20, "count");
                del(20, fromQQ.ToString());
                insert(20, "count", picCount.ToString());
                insert(20, fromQQ.ToString(), ConvertDateTimeInt(DateTime.Now).ToString());

                CQ.SendPrivateMessage(fromQQ, "图片修改完成！使用命令pixel查看");
            }
            else
            {
                CQ.SendPrivateMessage(fromQQ, "人家不认识你了啦");
            }

        }

        public static bool CheckID(string t)
        {
            t = t.ToUpper();
            if (t == "" || t == "ID")
                return false;
            t = t.Replace("A", "");
            t = t.Replace("B", "");
            t = t.Replace("C", "");
            t = t.Replace("D", "");
            t = t.Replace("E", "");
            t = t.Replace("F", "");
            t = t.Replace("G", "");
            t = t.Replace("H", "");
            t = t.Replace("I", "");
            t = t.Replace("J", "");
            t = t.Replace("K", "");
            t = t.Replace("L", "");
            t = t.Replace("M", "");
            t = t.Replace("N", "");
            t = t.Replace("O", "");
            t = t.Replace("P", "");
            t = t.Replace("Q", "");
            t = t.Replace("R", "");
            t = t.Replace("S", "");
            t = t.Replace("T", "");
            t = t.Replace("U", "");
            t = t.Replace("V", "");
            t = t.Replace("W", "");
            t = t.Replace("X", "");
            t = t.Replace("Y", "");
            t = t.Replace("Z", "");
            t = t.Replace("_", "");
            t = t.Replace("1", "");
            t = t.Replace("2", "");
            t = t.Replace("3", "");
            t = t.Replace("4", "");
            t = t.Replace("5", "");
            t = t.Replace("6", "");
            t = t.Replace("7", "");
            t = t.Replace("8", "");
            t = t.Replace("9", "");
            t = t.Replace("0", "");
            if (t == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static string RemoveColorCode(string s)
        {
            s.Replace("§1", "");
            s.Replace("§2", "");
            s.Replace("§3", "");
            s.Replace("§4", "");
            s.Replace("§5", "");
            s.Replace("§6", "");
            s.Replace("§7", "");
            s.Replace("§8", "");
            s.Replace("§9", "");
            s.Replace("§0", "");
            s.Replace("§a", "");
            s.Replace("§b", "");
            s.Replace("§c", "");
            s.Replace("§d", "");
            s.Replace("§e", "");
            s.Replace("§f", "");
            s.Replace("§l", "");
            s.Replace("§m", "");
            s.Replace("§n", "");
            return s;
        }

        public static void SendMinecraftMessage(long fromGroup, string msg)
        {
            if(fromGroup == 241464054)
            {
                CQ.SendGroupMessage(fromGroup, msg);
                mcmsg += "|||||[群消息]<接待喵>" + msg;
            }
            else
            {
                CQ.SendGroupMessage(fromGroup, msg);
            }
        }

        //qq群列表
        private static long[] grouplist = new long[] {
            115872123,
            150761455,
            185562383,
            194918236,
            195185733,
            209051300,
            215711032,
            241464054,
            26718371,
            293292761,
            327870533,
            339837275,
            372180029,
            376390309,
            383541727,
            389949154,
            431458615,
            438514287,
            456413726,
            458890830,
            464961621,
            485100909,
            523832787,
            544426661,
            567145439,
            567477706,
            582844145,
            59994612,
            83655291 };

        /// <summary>
        /// Type=2 群消息。
        /// </summary>
        /// <param name="subType">子类型，目前固定为1。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromGroup">来源群号。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="fromAnonymous">来源匿名者。</param>
        /// <param name="msg">消息内容。</param>
        /// <param name="font">字体。</param>
        public override void GroupMessage(int subType, int sendTime, long fromGroup, long fromQQ, string fromAnonymous, string msg, int font)
        {
            if (fromQQ == 80000000)
                return;
            // 处理群消息。
            var groupMember = CQ.GetGroupMemberInfo(fromGroup, fromQQ);
            var me = CQ.GetGroupMemberInfo(fromGroup, 751323264);
            if (fromGroup == 241464054 && fromQQ != 1000000)
            {
                if(msg.IndexOf("刘晨旭") != -1 || msg.IndexOf("艹") != -1 || msg.IndexOf("你妈") != -1)
                {
                    CQ.SetGroupMemberGag(fromGroup, fromQQ, 60*60);
                }
                string reply;
                string reply1 = xml_get(1, fromQQ.ToString());
                string reply2 = xml_get(5, fromQQ.ToString());
                if (reply1 != "")
                    reply = reply1;
                else
                    reply = reply2;
                if (fromQQ == 961726194 && msg.IndexOf("命令") == 0)
                {
                    mcmsg += "|||||command>" + msg.Replace("命令","");
                }
                else if (msg.IndexOf("命令") == 0)
                {
                    if (reply != "")
                    {
                        CQ.SendGroupMessage(fromGroup, "封禁" + reply + "命令执行成功！");
                    }
                }
                else if(msg == "在线")
                {

                    string CoinStr = xml_get(2, fromQQ.ToString());
                    int CoinsTemp;
                    if (CoinStr != "")
                    {
                        CoinsTemp = int.Parse(CoinStr);
                    }
                    else
                    {
                        CoinsTemp = 0;
                    }
                    if(CoinsTemp < 100)
                    {
                        SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "\r\n群银行游戏币不足100，无法查询在线人数");
                    }
                    else
                    {
                        CoinsTemp -= 100;
                        mcmsg += "|||||sum>我要看人数";
                        mcmsg += "|||||[群消息]<" + reply + ">" + msg;
                        del(2, fromQQ.ToString());
                        insert(2, fromQQ.ToString(), CoinsTemp.ToString());
                        SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "\r\n已扣除100游戏币作为查询在线人数的服务费");
                    }
                }
                else if (reply != "")
                {
                    mcmsg += "|||||[群消息]<" + reply + ">" + msg;
                    if (groupMember.GroupCard.IndexOf(reply) == -1)
                    {
                        CQ.SetGroupNickName(fromGroup, fromQQ, reply);
                    }
                }
                else if (msg.IndexOf("绑定") == 0)
                {
                    if (xml_get(1, fromQQ.ToString()) == "" && xml_get(5, fromQQ.ToString()) == "" && qq_get(msg.Replace("绑定", "")) == 0 && CheckID(msg.Replace("绑定", "")))
                    {
                        insert(5, fromQQ.ToString(), msg.Replace("绑定", ""));
                        SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "绑定id:" + msg.Replace("绑定", "") + "成功！" +
                                                      "\r\n请耐心等待管理员审核白名单申请哟~" +
                                                      "\r\n如未申请请打开此链接：https://wj.qq.com/s/1308067/143c" +
                                                      "\r\n如果过去24小时仍未被审核，请回复“催促审核”来进行催促");

                        CQ.SendGroupMessage(567145439, "接待喵糖拌管理：\r\n玩家id：" + msg.Replace("绑定", "") + "\r\n已成功绑定QQ：" + fromQQ.ToString() +
                                                        "\r\n群名片：" + groupMember.GroupCard +
                                                        "\r\n请及时检查该玩家是否已经提交白名单申请https://wj.qq.com/mine.html" +
                                                        "\r\n如果符合要求，请回复“通过”+qq来给予白名单" +
                                                        "\r\n如果不符合要求，请回复“不通过”+qq+空格+原因来给打回去重填");
                    }
                    else if (!CheckID(msg.Replace("绑定", "")))
                    {
                        SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "拜托你起个正常的名字好吗？");
                    }
                    else
                    {
                        SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "你可能已经绑定过了，请私聊腐竹解绑。");
                    }
                }
                else
                {
                    CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + "\r\n检测到你没有绑定服务器id，请在群里发送“绑定”+“你自己的id”来绑定（没空格），如：\r\n绑定notch\r\n长时间未绑定你将会被移出本群");
                    //CQ.SendGroupMessage(567145439, "接待喵糖拌管理：\r\nQQ：" + fromQQ.ToString() +
                    //                            "\r\n群名片：" + groupMember.GroupCard +
                    //                            "\r\n没有绑定id\r\n如长时间没绑请将其移出群");
                    return;
                }

                if (msg == "签到")
                {
                    if (xml_get(3, fromQQ.ToString()) == System.DateTime.Today.ToString())
                    {
                        SendMinecraftMessage(241464054, CQ.CQCode_At(fromQQ) + "你今天已经签过到啦！");
                    }
                    else
                    {
                        string last_time = xml_get(3, fromQQ.ToString());
                        string qdTimesStr = xml_get(7, fromQQ.ToString());
                        string CoinStr = xml_get(2, fromQQ.ToString());
                        int CoinsTemp, qdTimesTemp;
                        if (CoinStr != "")
                        {
                            CoinsTemp = int.Parse(CoinStr);
                        }
                        else
                        {
                            CoinsTemp = 0;
                        }
                        if (qdTimesStr != "")
                        {
                            qdTimesTemp = int.Parse(qdTimesStr);
                        }
                        else
                        {
                            qdTimesTemp = 1;
                        }
                        if(xml_get(3, fromQQ.ToString()) == System.DateTime.Today.AddDays(-1).ToString())
                        {
                            qdTimesTemp++;
                        }
                        else
                        {
                            qdTimesTemp = 1;
                        }
                        Random ran = new Random(System.DateTime.Now.Millisecond);
                        int RandKey = ran.Next(0, 100);
                        CoinsTemp += RandKey + qdTimesTemp;
                        SendMinecraftMessage(241464054, CQ.CQCode_At(fromQQ) + "\r\n签到成功！已连续签到"+ qdTimesTemp.ToString() + "天\r\n获得游戏币" + RandKey + "+"+ qdTimesTemp.ToString() + "枚！\r\n银行内游戏币" + CoinsTemp + "枚\r\n抽奖次数已重置为一次！\r\n回复“帮助”查看如何取钱\r\n如果登陆游戏进行签到的话可以获得五倍金币、五次抽奖机会哦~");
                        del(2, fromQQ.ToString());
                        del(3, fromQQ.ToString());
                        del(4, fromQQ.ToString());
                        del(7, fromQQ.ToString());
                        insert(2, fromQQ.ToString(), CoinsTemp.ToString());
                        insert(7, fromQQ.ToString(), qdTimesTemp.ToString());
                        insert(3, fromQQ.ToString(), System.DateTime.Today.ToString());
                        insert(4, fromQQ.ToString(), "4");

                        string qd_get = xml_get(8, "qd");  //签到数
                        int qd = 0;
                        try
                        {
                            qd = int.Parse(qd_get);
                        }
                        catch { }
                        qd++;
                        del(8, "qd");
                        insert(8, "qd", qd.ToString());
                    }
                }
                if (msg == "取钱" || msg == "我要取钱")
                {
                    string CoinStr = xml_get(2, fromQQ.ToString());
                    int CoinsTemp;
                    if (CoinStr != "")
                    {
                        CoinsTemp = int.Parse(CoinStr);
                    }
                    else
                    {
                        CoinsTemp = 0;
                    }
                    mcmsg += "|||||command>eco give " + reply + " " + CoinsTemp.ToString();
                    //SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "已为玩家" + reply + "充值" + CoinsTemp.ToString() + "游戏币！");
                    del(2, fromQQ.ToString());
                }
                if (msg == "查询" || msg == "查询余额")
                {
                    string CoinStr = xml_get(2, fromQQ.ToString());
                    int CoinsTemp;
                    if (CoinStr != "")
                    {
                        CoinsTemp = int.Parse(CoinStr);
                    }
                    else
                    {
                        CoinsTemp = 0;
                    }
                    SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "\r\n你当前余额为" + CoinsTemp.ToString() + "游戏币");
                    mcmsg += "|||||eco>" + reply;
                }
                if (msg == "存钱100")
                {
                    mcmsg += "|||||ecodel100>" + reply;
                }
                else if (msg == "存钱500")
                {
                    mcmsg += "|||||ecodel500>" + reply;
                }
                else if (msg == "存钱1000")
                {
                    mcmsg += "|||||ecodel1000>" + reply;
                }
                else if (msg == "存钱5000")
                {
                    mcmsg += "|||||ecodel5000>" + reply;
                }
                else if (msg == "存钱10000")
                {
                    mcmsg += "|||||ecodel10000>" + reply;
                }
                else if (msg.IndexOf("存钱") == 0)
                {
                    SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "无效命令，回复帮助查看更多。");
                }
                if (msg == "帮助")
                {
                    SendMinecraftMessage(fromGroup, "关于签到奖励：\r\n每日可随机领取0-500游戏币的奖励，奖励暂存在银行中。\r\n取钱方式：在群里发送“取钱”即可取出所有钱。\r\n回复存钱+金额可将服务器钱存入银行\r\n每次仅限100、500、1000、5000、10000这几个额度\r\n回复“查询”可查询当前银行内余额。\r\n回复“抽奖”可花费100游戏币进行抽奖，中奖率为玄学");
                }
                if (msg == "抽奖")
                {
                    string CoinStr = xml_get(2, fromQQ.ToString());
                    string RanCount = xml_get(4, fromQQ.ToString());
                    int CoinsTemp, Counttemp;
                    if (CoinStr != "")
                    {
                        CoinsTemp = int.Parse(CoinStr);
                    }
                    else
                    {
                        CoinsTemp = 0;
                    }

                    if (RanCount != "")
                    {
                        Counttemp = int.Parse(RanCount);
                    }
                    else
                    {
                        Counttemp = 0;
                    }

                    if (Counttemp < 5)
                    {
                        if (CoinsTemp < 100)
                        {
                            SendMinecraftMessage(fromGroup, "你只有" + CoinsTemp + "游戏币，你个穷逼还想抽奖？");
                        }
                        else
                        {
                            Random ran = new Random(System.DateTime.Now.Millisecond);
                            int RandKey = ran.Next(0, 500);
                            CoinsTemp -= 100;
                            if (RandKey > 350)
                            {
                                CoinsTemp += RandKey;
                                SendMinecraftMessage(fromGroup, "恭喜你抽中了" + RandKey + "游戏币，当前余额" + CoinsTemp + "游戏币");
                                del(2, fromQQ.ToString());
                                insert(2, fromQQ.ToString(), CoinsTemp.ToString());
                            }
                            else if(RandKey > 150)
                            {
                                SendMinecraftMessage(fromGroup, "啊哦，你没有中奖。当前余额" + CoinsTemp + "游戏币");
                                del(2, fromQQ.ToString());
                                insert(2, fromQQ.ToString(), CoinsTemp.ToString());
                            }
                            else if(RandKey > 50)
                            {
                                int fkt = ran.Next(1, 11);
                                CQ.SetGroupMemberGag(fromGroup, fromQQ, fkt * 60);
                                SendMinecraftMessage(fromGroup, "恭喜你抽中了禁言" + fkt + "分钟！当前余额" + CoinsTemp + "游戏币");
                                del(2, fromQQ.ToString());
                                insert(2, fromQQ.ToString(), CoinsTemp.ToString());
                            }
                            else
                            {
                                int fk = 0;
                                string fks = xml_get(10, fromQQ.ToString());
                                if(fks != "")
                                {
                                    fk = int.Parse(fks);
                                }
                                fk++;
                                SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "\r\n恭喜你抽中了一张禁言卡，回复“禁言卡”可以查看使用帮助。");
                                del(10, fromQQ.ToString());
                                insert(10, fromQQ.ToString(), fk.ToString());
                                del(2, fromQQ.ToString());
                                insert(2, fromQQ.ToString(), CoinsTemp.ToString());
                            }
                            Counttemp++;
                            del(4, fromQQ.ToString());
                            insert(4, fromQQ.ToString(), Counttemp.ToString());
                        }
                    }
                    else
                    {
                        SendMinecraftMessage(fromGroup, "抽奖次数已用完！请用签到补充次数");
                    }

                }
                if (msg == "5连抽" || msg == "五连抽")
                {
                    string CoinStr = xml_get(2, fromQQ.ToString());
                    string RanCount = xml_get(4, fromQQ.ToString());
                    int CoinsTemp, Counttemp;
                    if (CoinStr != "")
                    {
                        CoinsTemp = int.Parse(CoinStr);
                    }
                    else
                    {
                        CoinsTemp = 0;
                    }

                    if (RanCount != "")
                    {
                        Counttemp = int.Parse(RanCount);
                    }
                    else
                    {
                        Counttemp = 0;
                    }

                    if (Counttemp < 1)
                    {
                        if (CoinsTemp < 500)
                        {
                            SendMinecraftMessage(fromGroup, "你只有" + CoinsTemp + "游戏币，你个穷逼还想五连抽？");
                        }
                        else
                        {
                            CoinsTemp -= 500;
                            del(2, fromQQ.ToString());
                            insert(2, fromQQ.ToString(), CoinsTemp.ToString());
                            Random ran = new Random(System.DateTime.Now.Millisecond);
                            string replay5all = "";
                            int RandKey,jinyan = 0;
                            for(int count_temp = 0;count_temp<5;count_temp++)
                            {
                                RandKey = ran.Next(0, 500);
                                if (RandKey > 350)
                                {
                                    CoinsTemp += RandKey;
                                    replay5all += "恭喜你抽中了" + RandKey + "游戏币，当前余额" + CoinsTemp + "游戏币\r\n";
                                }
                                else if (RandKey > 150)
                                {
                                    replay5all += "啊哦，你没有中奖。当前余额" + CoinsTemp + "游戏币\r\n";
                                }
                                else if (RandKey > 50)
                                {
                                    int fkt = ran.Next(1, 11);
                                    jinyan += fkt;
                                    replay5all += "恭喜你抽中了禁言" + fkt + "分钟！当前余额" + CoinsTemp + "游戏币\r\n";
                                }
                                else
                                {
                                    int fk = 0;
                                    string fks = xml_get(10, fromQQ.ToString());
                                    if (fks != "")
                                    {
                                        fk = int.Parse(fks);
                                    }
                                    fk++;
                                    replay5all += "恭喜你抽中了一张禁言卡，回复“禁言卡”可以查看使用帮助。\r\n";
                                    del(10, fromQQ.ToString());
                                    insert(10, fromQQ.ToString(), fk.ToString());
                                }
                            }
                            Counttemp+=5;
                            del(4, fromQQ.ToString());
                            del(2, fromQQ.ToString());
                            insert(4, fromQQ.ToString(), Counttemp.ToString());
                            insert(2, fromQQ.ToString(), CoinsTemp.ToString());
                            SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "\r\n" + replay5all + "五连抽完毕");
                            if (jinyan > 0)
                            {
                                CQ.SetGroupMemberGag(fromGroup, fromQQ, jinyan * 60);
                            }
                        }
                    }
                    else
                    {
                        SendMinecraftMessage(fromGroup, "你今天只有"+ (5 - Counttemp).ToString() + "次抽奖机会了，无法进行五连抽");
                    }

                }
                if (msg == "4连抽" || msg == "四连抽")
                {
                    string CoinStr = xml_get(2, fromQQ.ToString());
                    string RanCount = xml_get(4, fromQQ.ToString());
                    int CoinsTemp, Counttemp;
                    if (CoinStr != "")
                    {
                        CoinsTemp = int.Parse(CoinStr);
                    }
                    else
                    {
                        CoinsTemp = 0;
                    }

                    if (RanCount != "")
                    {
                        Counttemp = int.Parse(RanCount);
                    }
                    else
                    {
                        Counttemp = 0;
                    }

                    if (Counttemp < 2)
                    {
                        if (CoinsTemp < 400)
                        {
                            SendMinecraftMessage(fromGroup, "你只有" + CoinsTemp + "游戏币，你个穷逼还想四连抽？");
                        }
                        else
                        {
                            CoinsTemp -= 400;
                            del(2, fromQQ.ToString());
                            insert(2, fromQQ.ToString(), CoinsTemp.ToString());
                            Random ran = new Random(System.DateTime.Now.Millisecond);
                            string replay5all = "";
                            int RandKey, jinyan = 0;
                            for (int count_temp = 0; count_temp < 4; count_temp++)
                            {
                                RandKey = ran.Next(0, 500);
                                if (RandKey > 350)
                                {
                                    CoinsTemp += RandKey;
                                    replay5all += "恭喜你抽中了" + RandKey + "游戏币，当前余额" + CoinsTemp + "游戏币\r\n";
                                }
                                else if (RandKey > 150)
                                {
                                    replay5all += "啊哦，你没有中奖。当前余额" + CoinsTemp + "游戏币\r\n";
                                }
                                else if (RandKey > 50)
                                {
                                    int fkt = ran.Next(1, 11);
                                    jinyan += fkt;
                                    replay5all += "恭喜你抽中了禁言" + fkt + "分钟！当前余额" + CoinsTemp + "游戏币\r\n";
                                }
                                else
                                {
                                    int fk = 0;
                                    string fks = xml_get(10, fromQQ.ToString());
                                    if (fks != "")
                                    {
                                        fk = int.Parse(fks);
                                    }
                                    fk++;
                                    replay5all += "恭喜你抽中了一张禁言卡，回复“禁言卡”可以查看使用帮助。\r\n";
                                    del(10, fromQQ.ToString());
                                    insert(10, fromQQ.ToString(), fk.ToString());
                                }
                            }
                            Counttemp += 4;
                            del(4, fromQQ.ToString());
                            del(2, fromQQ.ToString());
                            insert(4, fromQQ.ToString(), Counttemp.ToString());
                            insert(2, fromQQ.ToString(), CoinsTemp.ToString());
                            SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "\r\n" + replay5all + "四连抽完毕");
                            if (jinyan > 0)
                            {
                                CQ.SetGroupMemberGag(fromGroup, fromQQ, jinyan * 60);
                            }
                        }
                    }
                    else
                    {
                        SendMinecraftMessage(fromGroup, "你今天只有" + (5 - Counttemp).ToString() + "次抽奖机会了，无法进行四连抽");
                    }

                }
                if (msg == "3连抽" || msg == "三连抽")
                {
                    string CoinStr = xml_get(2, fromQQ.ToString());
                    string RanCount = xml_get(4, fromQQ.ToString());
                    int CoinsTemp, Counttemp;
                    if (CoinStr != "")
                    {
                        CoinsTemp = int.Parse(CoinStr);
                    }
                    else
                    {
                        CoinsTemp = 0;
                    }

                    if (RanCount != "")
                    {
                        Counttemp = int.Parse(RanCount);
                    }
                    else
                    {
                        Counttemp = 0;
                    }

                    if (Counttemp < 3)
                    {
                        if (CoinsTemp < 300)
                        {
                            SendMinecraftMessage(fromGroup, "你只有" + CoinsTemp + "游戏币，你个穷逼还想三连抽？");
                        }
                        else
                        {
                            CoinsTemp -= 300;
                            del(2, fromQQ.ToString());
                            insert(2, fromQQ.ToString(), CoinsTemp.ToString());
                            Random ran = new Random(System.DateTime.Now.Millisecond);
                            string replay5all = "";
                            int RandKey, jinyan = 0;
                            for (int count_temp = 0; count_temp < 3; count_temp++)
                            {
                                RandKey = ran.Next(0, 500);
                                if (RandKey > 350)
                                {
                                    CoinsTemp += RandKey;
                                    replay5all += "恭喜你抽中了" + RandKey + "游戏币，当前余额" + CoinsTemp + "游戏币\r\n";
                                }
                                else if (RandKey > 150)
                                {
                                    replay5all += "啊哦，你没有中奖。当前余额" + CoinsTemp + "游戏币\r\n";
                                }
                                else if (RandKey > 50)
                                {
                                    int fkt = ran.Next(1, 11);
                                    jinyan += fkt;
                                    replay5all += "恭喜你抽中了禁言" + fkt + "分钟！当前余额" + CoinsTemp + "游戏币\r\n";
                                }
                                else
                                {
                                    int fk = 0;
                                    string fks = xml_get(10, fromQQ.ToString());
                                    if (fks != "")
                                    {
                                        fk = int.Parse(fks);
                                    }
                                    fk++;
                                    replay5all += "恭喜你抽中了一张禁言卡，回复“禁言卡”可以查看使用帮助。\r\n";
                                    del(10, fromQQ.ToString());
                                    insert(10, fromQQ.ToString(), fk.ToString());
                                }
                            }
                            Counttemp += 3;
                            del(4, fromQQ.ToString());
                            del(2, fromQQ.ToString());
                            insert(4, fromQQ.ToString(), Counttemp.ToString());
                            insert(2, fromQQ.ToString(), CoinsTemp.ToString());
                            SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "\r\n" + replay5all + "三连抽完毕");
                            if (jinyan > 0)
                            {
                                CQ.SetGroupMemberGag(fromGroup, fromQQ, jinyan * 60);
                            }
                        }
                    }
                    else
                    {
                        SendMinecraftMessage(fromGroup, "你今天只有" + (5 - Counttemp).ToString() + "次抽奖机会了，无法进行三连抽");
                    }

                }
                if (msg == "2连抽" || msg == "二连抽" || msg == "两连抽")
                {
                    string CoinStr = xml_get(2, fromQQ.ToString());
                    string RanCount = xml_get(4, fromQQ.ToString());
                    int CoinsTemp, Counttemp;
                    if (CoinStr != "")
                    {
                        CoinsTemp = int.Parse(CoinStr);
                    }
                    else
                    {
                        CoinsTemp = 0;
                    }

                    if (RanCount != "")
                    {
                        Counttemp = int.Parse(RanCount);
                    }
                    else
                    {
                        Counttemp = 0;
                    }

                    if (Counttemp < 4)
                    {
                        if (CoinsTemp < 200)
                        {
                            SendMinecraftMessage(fromGroup, "你只有" + CoinsTemp + "游戏币，你个穷逼还想两连抽？");
                        }
                        else
                        {
                            CoinsTemp -= 200;
                            del(2, fromQQ.ToString());
                            insert(2, fromQQ.ToString(), CoinsTemp.ToString());
                            Random ran = new Random(System.DateTime.Now.Millisecond);
                            string replay5all = "";
                            int RandKey, jinyan = 0;
                            for (int count_temp = 0; count_temp < 2; count_temp++)
                            {
                                RandKey = ran.Next(0, 500);
                                if (RandKey > 350)
                                {
                                    CoinsTemp += RandKey;
                                    replay5all += "恭喜你抽中了" + RandKey + "游戏币，当前余额" + CoinsTemp + "游戏币\r\n";
                                }
                                else if (RandKey > 150)
                                {
                                    replay5all += "啊哦，你没有中奖。当前余额" + CoinsTemp + "游戏币\r\n";
                                }
                                else if (RandKey > 50)
                                {
                                    int fkt = ran.Next(1, 11);
                                    jinyan += fkt;
                                    replay5all += "恭喜你抽中了禁言" + fkt + "分钟！当前余额" + CoinsTemp + "游戏币\r\n";
                                }
                                else
                                {
                                    int fk = 0;
                                    string fks = xml_get(10, fromQQ.ToString());
                                    if (fks != "")
                                    {
                                        fk = int.Parse(fks);
                                    }
                                    fk++;
                                    replay5all += "恭喜你抽中了一张禁言卡，回复“禁言卡”可以查看使用帮助。\r\n";
                                    del(10, fromQQ.ToString());
                                    insert(10, fromQQ.ToString(), fk.ToString());
                                }
                            }
                            Counttemp += 2;
                            del(4, fromQQ.ToString());
                            del(2, fromQQ.ToString());
                            insert(4, fromQQ.ToString(), Counttemp.ToString());
                            insert(2, fromQQ.ToString(), CoinsTemp.ToString());
                            SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "\r\n" + replay5all + "两连抽完毕");
                            if (jinyan > 0)
                            {
                                CQ.SetGroupMemberGag(fromGroup, fromQQ, jinyan * 60);
                            }
                        }
                    }
                    else
                    {
                        SendMinecraftMessage(fromGroup, "你今天只有" + (5 - Counttemp).ToString() + "次抽奖机会了，无法进行两连抽");
                    }

                }
                if (msg == "激活")
                {
                    if(reply1 != "")
                    {
                        mcmsg += "|||||command>manuadd " + reply1 + " builder world";
                        SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "已给予你的账号相应权限");
                    }
                    else
                    {
                        SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "你没有通过白名单审核，无法激活账号");
                    }
                }
                if (msg.IndexOf("邀请人") == 0)
                {
                    string reply3 = xml_get(6, fromQQ.ToString());
                    if (reply1 == "")
                    {
                        SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "你还没有白名单呢！");
                        return;
                    }
                    else if (reply3 != "")
                    {
                        SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "已领取过该奖励，请勿再次领取");
                        return;
                    }
                    long targetQQ = 0;
                    try
                    {
                        targetQQ = GetNumberLong(msg);
                        if (fromQQ == targetQQ)
                        {
                            SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "。。。。");
                            SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "不要这样子好吗。。。。哪有自己邀请自己的。。。");
                            return;
                        }
                        string CoinStr1 = xml_get(2, targetQQ.ToString());
                        int CoinsTemp1;
                        if (CoinStr1 != "")
                        {
                            CoinsTemp1 = int.Parse(CoinStr1);
                        }
                        else
                        {
                            CoinsTemp1 = 0;
                        }

                        string CoinStr2 = xml_get(2, fromQQ.ToString());
                        int CoinsTemp2;
                        if (CoinStr2 != "")
                        {
                            CoinsTemp2 = int.Parse(CoinStr2);
                        }
                        else
                        {
                            CoinsTemp2 = 0;
                        }

                        CoinsTemp1 += 3000;
                        CoinsTemp2 += 1000;
                        del(2, fromQQ.ToString());
                        del(2, targetQQ.ToString());
                        insert(2, fromQQ.ToString(), CoinsTemp2.ToString());
                        insert(2, targetQQ.ToString(), CoinsTemp1.ToString());
                        insert(6, fromQQ.ToString(), "推荐人：" + targetQQ.ToString());
                        SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "游戏币奖励已发放到群内银行~给你1000，给你的小伙伴3000~快去告诉你的小伙伴吧~");
                    }
                    catch
                    {
                        SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "命令错误！");
                    }
                }
                if (msg == "催促审核")
                {
                    string reply3 = xml_get(5, fromQQ.ToString());
                    if (reply1 != "")
                    {
                        SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "你已经有白名单了");
                        return;
                    }
                    else if (reply3 == "")
                    {
                        SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "你还没有绑定id，请回复“绑定”加你的id来绑定");
                        return;
                    }
                    CQ.SendGroupMessage(567145439, "接待喵糖拌管理：\r\n玩家id：" + reply3 + "\r\nQQ：" + fromQQ.ToString() +
                                                "\r\n进行了催促审核的操作" +
                                                "\r\n请及时检查该玩家是否已经提交白名单申请https://wj.qq.com/mine.html" +
                                                "\r\n如果符合要求，请回复“通过”+qq来给予白名单" +
                                                "\r\n如果不符合要求，请回复“不通过”+qq+空格+原因来给打回去重填" +
                                                "\r\n" + CQ.CQCode_At(1021479600) + CQ.CQCode_At(635309406) +
                                                CQ.CQCode_At(1928361196) + CQ.CQCode_At(1420355171) + CQ.CQCode_At(280585112) +
                                                CQ.CQCode_At(2561620740) + CQ.CQCode_At(2433380978) + CQ.CQCode_At(2679146075) +
                                                CQ.CQCode_At(961726194) + CQ.CQCode_At(185939950));
                    SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "已成功催促管理员审核！请耐心等待！如果还没有被审核，你可以选择继续催促！");
                }
            }  //糖拌群
            if (fromGroup == 567145439)
            {
                if (msg.IndexOf("删除") == 0)
                {
                    if (msg.Replace("删除","") != "")
                    {
                        del(1, msg.Replace("删除", ""));
                        del(5, msg.Replace("删除", ""));
                        SendMinecraftMessage(fromGroup, "已删除QQ：" + msg.Replace("删除", "") + "所绑定的id。");
                    }
                }
                else if (msg.IndexOf("通过") == 0)
                {
                    string reply = xml_get(5, msg.Replace("通过", ""));
                    if(reply!="")
                    {
                        del(5, msg.Replace("通过", ""));
                        insert(1, msg.Replace("通过", ""), reply);
                        SendMinecraftMessage(fromGroup, "已通过QQ：" + msg.Replace("通过", "") + "，id：" + reply + "的白名单申请");
                        SendMinecraftMessage(241464054, CQ.CQCode_At(long.Parse(msg.Replace("通过", ""))) + "你的白名单申请已经通过了哟~"+
                                                                                "\r\n当游戏在线的时候在群里发送“激活”即可激活你的账号哦~"+
                                                                                "\r\n你的id：" + reply +
                                                                                "\r\n现在回复我“邀请人”加上@邀请人" +
                                                                                "\r\n可以让邀请者和被邀请者领取奖励哟~");
                    }
                    else
                    {
                        SendMinecraftMessage(fromGroup, "参数不对或该玩家不在待审核玩家数据中");
                    }
                    
                }
                else if (msg.IndexOf("不通过") == 0)
                {
                    if (msg.IndexOf("不通过 ") == 0)
                    {
                        SendMinecraftMessage(fromGroup, "命令关键字后不要加空格，直接加玩家id");
                        return;
                    }
                    string reply = "";
                    string reason = "";
                    string qq_get = "";
                    string[] str2;
                    int count_temp = 0;
                    str2 = msg.Replace("不通过","").Split(' ');
                    foreach (string i in str2)
                    {
                        if (count_temp == 0)
                        {
                            reply = xml_get(5, i);
                            qq_get = i;
                            count_temp++;
                        }
                        else if (count_temp == 1)
                        {
                            reason += i + " ";
                        }
                    }
                    
                    if (reply != "")
                    {
                        SendMinecraftMessage(fromGroup, "已不通过QQ：" + qq_get + "，id：" + reply + "的白名单申请，原因：" + reason);
                        SendMinecraftMessage(241464054, CQ.CQCode_At(long.Parse(qq_get)) + "你的白名单申请并没有通过。" +
                                                                                "\r\n原因：" + reason +
                                                                                "\r\n请按照原因重新填写白名单：https://wj.qq.com/s/1308067/143c" +
                                                                                "\r\n你的id：" + reply);
                    }
                    else
                    {
                        SendMinecraftMessage(fromGroup, "参数不对或该玩家不在待审核玩家数据中");
                    }

                }
                else if (msg.IndexOf("封禁") == 0)
                {
                    if (msg.IndexOf("封禁 ") == 0)
                    {
                        SendMinecraftMessage(fromGroup, "命令关键字后不要加空格，直接加玩家id");
                        return;
                    }
                    string reason = "";
                    string id_get = "";
                    string[] str2;
                    int count_temp = 0;
                    str2 = msg.Replace("封禁", "").Split(' ');
                    foreach (string i in str2)
                    {
                        if (count_temp == 0)
                        {
                            id_get = i;
                            count_temp++;
                        }
                        else if (count_temp == 1)
                        {
                            reason += i + " ";
                        }
                    }

                    SendMinecraftMessage(fromGroup, "已封禁id：" + id_get + "原因：" + reason);
                    mcmsg += "|||||command>ban " + id_get + " " + reason;
                    SendMinecraftMessage(241464054, "玩家" + id_get + "已被管理员封禁\r\n" + "封禁原因：" + reason + "\r\n如有误判请联系管理");
                }
                else if (msg.IndexOf("解封") == 0)
                {
                    if (msg.IndexOf("解封 ") == 0)
                    {
                        SendMinecraftMessage(fromGroup, "命令关键字后不要加空格，直接加玩家id");
                        return;
                    }
                    string id_get = msg.Replace("解封", "");

                    SendMinecraftMessage(fromGroup, "已解除玩家" + id_get + "的封禁");
                    mcmsg += "|||||command>unban " + id_get;
                    SendMinecraftMessage(241464054, "玩家" + id_get + "已被管理员解除封禁");
                }
                else if (msg.IndexOf("踢出") == 0)
                {
                    if (msg.IndexOf("踢出 ") == 0)
                    {
                        SendMinecraftMessage(fromGroup, "命令关键字后不要加空格，直接加玩家id");
                        return;
                    }
                    string reason = "";
                    string id_get = "";
                    string[] str2;
                    int count_temp = 0;
                    str2 = msg.Replace("踢出", "").Split(' ');
                    foreach (string i in str2)
                    {
                        if (count_temp == 0)
                        {
                            id_get = i;
                            count_temp++;
                        }
                        else if (count_temp == 1)
                        {
                            reason += i + " ";
                        }
                    }

                    SendMinecraftMessage(fromGroup, "已将玩家" + id_get + "踢出服务器\r\n原因：" + reason);
                    mcmsg += "|||||command>kick " + id_get + " " + reason;
                    SendMinecraftMessage(241464054, "玩家" + id_get + "已被管理员踢出服务器\r\n原因：" + reason);
                }

            }  //糖拌管理群


            //CQ.SendGroupMessage(fromGroup, String.Format("[{4}]{0} 你的群名片：{1}， 入群时间：{2}， 最后发言：{3}。", CQ.CQCode_At(fromQQ),
            //    groupMember.GroupCard, groupMember.JoinTime, groupMember.LastSpeakingTime, CQ.ProxyType));
            // CQ.SendGroupMessage(fromGroup, String.Format("[{0}]{1}你发的群消息是：{2}", CQ.ProxyType, CQ.CQCode_At(fromQQ), msg));
            //CQ.SendGroupMessage(fromGroup, string.Format("{0}发的群消息是：{1}", CQ.CQCode_At(fromQQ), msg));
            string replay_ok = replay_get(fromGroup, msg);
            string replay_common = replay_get(2333, msg);

            if (msg.ToUpper() == "HELP")
            {
                //CQ.SendGroupMessage(fromGroup, msg.ToUpper().IndexOf("help").ToString());
                SendMinecraftMessage(fromGroup, "命令帮助：\r\n！add 词条：回答\r\n！del 词条：回答\r\n！list 词条\r\n" +
                    "所有符号均为全角符号\r\n词条中请勿包含冒号\r\n" +
                    "发送“坷垃金曲”+数字序号即可点金坷垃歌（如坷垃金曲21，最大71）\r\n" +
                    "私聊发送“赞我”可使接待给你点赞\r\n" +
                    "发送“今日运势”可以查看今日运势\r\n" +
                    "发送“淘宝”+关键词即可搜索淘宝优惠搜索结果\r\n" +
                    "发送“pixel”可以查看像素游戏图片\r\n" +
                    "发送“查快递”和单号即可搜索快递物流信息\r\n" +
                    "发送“网易云”和歌曲id号/歌曲名即可定向点歌\r\n" +
                    "发送“正则”+字符串+“换行”+正则表达式，可查询C#正则\r\n" +
                    "如有bug请反馈");
            }
            else if (msg.IndexOf("点歌") == 0)
            {
                int song = 0;
                try
                {
                    song = int.Parse(msg.Replace("点歌", ""));
                }catch { }
                if (song >= 1 && song <= 134)
                {
                    SendMinecraftMessage(fromGroup, string.Format("{0}正在发送歌曲{1}，请稍候哦~\r\n如未收到请重试", CQ.CQCode_At(fromQQ), song.ToString()));
                    CQ.SendGroupMessage(fromGroup, "[CQ:record,file=CoolQ 语音时代！\\点歌\\" + song.ToString().PadLeft(3, '0') + ".mp3]");
                }
                else
                {
                    SendMinecraftMessage(fromGroup, string.Format("{0}编号不对哦，编号只能是1-134", CQ.CQCode_At(fromQQ)));
                }
            }
            else if (msg.IndexOf("坷垃金曲") == 0)
            {
                int song = 0;
                try
                {
                    song = int.Parse(msg.Replace("坷垃金曲", ""));
                }
                catch { }
                if (song >= 1 && song <= 71)
                {
                    SendMinecraftMessage(fromGroup, string.Format("{0}正在发送坷垃金曲{1}，请稍候哦~\r\n如未收到请重试", CQ.CQCode_At(fromQQ), song.ToString()));
                    CQ.SendGroupMessage(fromGroup, "[CQ:record,file=CoolQ 语音时代！\\坷垃金曲\\" + song.ToString().PadLeft(3, '0') + ".mp3]");
                }
                else
                {
                    SendMinecraftMessage(fromGroup, string.Format("{0}编号不对哦，编号只能是1-71", CQ.CQCode_At(fromQQ)));
                }
            }
            else if (msg == "赞我" || msg== "点赞")
            {
                //CQ.SendPraise(fromQQ, 10);
                CQ.SendPrivateMessage(fromQQ, "妈的智障以后私聊我点赞，别在群里发");
                //CQ.SendGroupMessage(fromGroup, "已为QQ" + fromQQ + "点赞十次");
            }
            else if (msg.IndexOf("！list ") == 0)
            {
                SendMinecraftMessage(fromGroup, string.Format("当前词条回复如下：\r\n{0}\r\n全局词库内容：\r\n{1}",
                                                                list_get(fromGroup, msg.Replace("！list ", "")),
                                                                list_get(2333, msg.Replace("！list ", "")) ));
            }
            else if (msg.IndexOf("！add ") == 0)
            {
                if (AdminCheck(fromQQ) == 1)
                {
                    string get_msg = msg.Replace("！add ", ""), tmsg = "", tans = "";

                    if (get_msg.IndexOf("：") >= 1 && get_msg.IndexOf("：") != get_msg.Length - 1)
                    {
                        string[] str2;
                        int count_temp = 0;
                        str2 = get_msg.Split('：');
                        foreach (string i in str2)
                        {
                            if (count_temp == 0)
                            {
                                tmsg = i.ToString();
                                count_temp++;
                            }
                            else if (count_temp == 1)
                            {
                                tans = i.ToString();
                            }
                        }
                        insert(fromGroup, tmsg, tans);
                        SendMinecraftMessage(fromGroup, "添加完成！\r\n词条：" + tmsg + "\r\n回答为：" + tans);
                    }
                    else
                    {
                        SendMinecraftMessage(fromGroup, "格式错误！");
                    }
                }
                else
                {
                    SendMinecraftMessage(fromGroup, "你没有权限调教接待喵");
                }
            }
            else if (msg.IndexOf("！del ") == 0)
            {
                if (AdminCheck(fromQQ) == 1)
                {
                    string get_msg = msg.Replace("！del ", ""), tmsg = "", tans = "";
                    if (get_msg.IndexOf("：") >= 1 && get_msg.IndexOf("：") != get_msg.Length - 1)
                    {
                        string[] str2;
                        int count_temp = 0;
                        str2 = get_msg.Split('：');
                        foreach (string i in str2)
                        {
                            if (count_temp == 0)
                            {
                                tmsg = i.ToString();
                                count_temp++;
                            }
                            else if (count_temp == 1)
                            {
                                tans = i.ToString();
                            }
                        }
                        remove(fromGroup, tmsg, tans);
                        SendMinecraftMessage(fromGroup, "删除完成！\r\n词条：" + tmsg + "\r\n回答为：" + tans);
                    }
                    else
                    {
                        SendMinecraftMessage(fromGroup, "格式错误！");
                    }
                }
                else
                {
                    SendMinecraftMessage(fromGroup, "你没有权限调教接待喵");
                }
            }
            else if(msg == "今日黄历" || msg == "今日运势")
            {
                if (fromQQ == 1262897311 || fromQQ == 66831919)
                {
                    string rs = string.Format("{0}\r\n你的今日运势如下~\r\n会被捅死。\r\n今日日期：{1}",
                            CQ.CQCode_At(fromQQ), System.DateTime.Today.ToString().Replace(" 0:00:00", "")
                            );
                    SendMinecraftMessage(fromGroup, rs);
                    return;
                }
                string ReplayString="";
                Random ran = new Random(System.DateTime.Now.DayOfYear + (int)(fromQQ - (fromQQ/10000) * 10000) );
                //int RanKey = ran.Next(0, 25);
                int sum1, sum2, sum3, sum4, bad1, bad2, count = 0;
                sum1 = ran.Next(0, 25);
                sum2 = ran.Next(0, 25);
                while (sum2 == sum1)
                {
                    sum2 = ran.Next(0, 25);
                    count++; if (count > 10) { sum2 = 25; break; }
                }count = 0;
                sum3 = ran.Next(0, 25);
                while (sum3 == sum1 || sum3 == sum2)
                {
                    sum3 = ran.Next(0, 25);
                    count++;if (count > 10) { sum3 = 25; break; }
                }count = 0;
                sum4 = ran.Next(0, 25);
                while (sum4 == sum1 || sum4 == sum2 || sum4 == sum3)
                {
                    sum4 = ran.Next(0, 25);
                    count++; if (count > 10) { sum4 = 25; break; }
                }count = 0;
                bad1 = ran.Next(0, 25);
                while (bad1 == sum1 || bad1 == sum2 || bad1 == sum3 || bad1 == sum4)
                {
                    bad2 = ran.Next(0, 25);
                    count++; if (count > 10) { bad1 = 25; break; }
                }count = 0;
                bad2 = ran.Next(0, 25);
                while (bad2 == bad1 || bad2 == sum1 || bad2 == sum2 || bad2 == sum3 || bad2 == sum4)
                {
                    bad2 = ran.Next(0, 25);
                    count++; if (count > 10) { bad2 = 25; break; }
                }count = 0;

                int allsum = ran.Next(0, 100);
                ReplayString = string.Format("{0}\r\n你的今日运势如下~\r\n宜：\r\n{1}\r\n{2}\r\n{3}\r\n{4}\r\n忌：\r\n{5}\r\n{6}\r\n今日日期：{7}\r\n今日综合幸运指数：{8}%\r\n今日吉言：{9}",
                                            CQ.CQCode_At(fromQQ),
                                            GoodThings[sum1],
                                            GoodThings[sum2],
                                            GoodThings[sum3],
                                            GoodThings[sum4],
                                            BadThings[bad1],
                                            BadThings[bad2],
                                            System.DateTime.Today.ToString().Replace(" 0:00:00", ""),
                                            allsum.ToString(),
                                            itsays[ran.Next(0,613)]
                                            );
                SendMinecraftMessage(fromGroup, ReplayString);
            }
            else if (msg == "昨日黄历" || msg == "昨日运势")
            {
                if(fromQQ== 1262897311 || fromQQ== 66831919)
                {
                    string rs = string.Format("{0}\r\n你的昨日运势如下~\r\n会被捅死。\r\n今日日期：{1}",
                            CQ.CQCode_At(fromQQ), System.DateTime.Today.ToString().Replace(" 0:00:00", "")
                            );
                    SendMinecraftMessage(fromGroup, rs);
                    return;
                }
                string ReplayString = "";
                Random ran = new Random(System.DateTime.Now.DayOfYear - 1 + (int)(fromQQ - (fromQQ / 10000) * 10000));
                //int RanKey = ran.Next(0, 25);
                int sum1, sum2, sum3, sum4, bad1, bad2, count = 0;
                sum1 = ran.Next(0, 25);
                sum2 = ran.Next(0, 25);
                while (sum2 == sum1)
                {
                    sum2 = ran.Next(0, 25);
                    count++; if (count > 10) { sum2 = 25; break; }
                }
                count = 0;
                sum3 = ran.Next(0, 25);
                while (sum3 == sum1 || sum3 == sum2)
                {
                    sum3 = ran.Next(0, 25);
                    count++; if (count > 10) { sum3 = 25; break; }
                }
                count = 0;
                sum4 = ran.Next(0, 25);
                while (sum4 == sum1 || sum4 == sum2 || sum4 == sum3)
                {
                    sum4 = ran.Next(0, 25);
                    count++; if (count > 10) { sum4 = 25; break; }
                }
                count = 0;
                bad1 = ran.Next(0, 25);
                while (bad1 == sum1 || bad1 == sum2 || bad1 == sum3 || bad1 == sum4)
                {
                    bad2 = ran.Next(0, 25);
                    count++; if (count > 10) { bad1 = 25; break; }
                }
                count = 0;
                bad2 = ran.Next(0, 25);
                while (bad2 == bad1 || bad2 == sum1 || bad2 == sum2 || bad2 == sum3 || bad2 == sum4)
                {
                    bad2 = ran.Next(0, 25);
                    count++; if (count > 10) { bad2 = 25; break; }
                }
                count = 0;
                ReplayString = string.Format("{0}\r\n你的昨日运势如下~\r\n宜：\r\n{1}\r\n{2}\r\n{3}\r\n{4}\r\n忌：\r\n{5}\r\n{6}\r\n今日日期：{7}",
                                            CQ.CQCode_At(fromQQ),
                                            GoodThings[sum1],
                                            GoodThings[sum2],
                                            GoodThings[sum3],
                                            GoodThings[sum4],
                                            BadThings[bad1],
                                            BadThings[bad2],
                                            System.DateTime.Today.ToString().Replace(" 0:00:00", "")
                                            );
                SendMinecraftMessage(fromGroup, ReplayString);
            }
            else if (msg == "明日黄历" || msg == "明日运势")
            {
                if (fromQQ == 1262897311 || fromQQ == 66831919)
                {
                    string rs = string.Format("{0}\r\n你的明日运势如下~\r\n会被捅死。\r\n今日日期：{1}",
                            CQ.CQCode_At(fromQQ), System.DateTime.Today.ToString().Replace(" 0:00:00", "")
                            );
                    SendMinecraftMessage(fromGroup, rs);
                    return;
                }
                string ReplayString = "";
                Random ran = new Random(System.DateTime.Now.DayOfYear + 1 + (int)(fromQQ - (fromQQ / 10000) * 10000));
                //int RanKey = ran.Next(0, 25);
                int sum1, sum2, sum3, sum4, bad1, bad2, count = 0;
                sum1 = ran.Next(0, 25);
                sum2 = ran.Next(0, 25);
                while (sum2 == sum1)
                {
                    sum2 = ran.Next(0, 25);
                    count++; if (count > 10) { sum2 = 25; break; }
                }
                count = 0;
                sum3 = ran.Next(0, 25);
                while (sum3 == sum1 || sum3 == sum2)
                {
                    sum3 = ran.Next(0, 25);
                    count++; if (count > 10) { sum3 = 25; break; }
                }
                count = 0;
                sum4 = ran.Next(0, 25);
                while (sum4 == sum1 || sum4 == sum2 || sum4 == sum3)
                {
                    sum4 = ran.Next(0, 25);
                    count++; if (count > 10) { sum4 = 25; break; }
                }
                count = 0;
                bad1 = ran.Next(0, 25);
                while (bad1 == sum1 || bad1 == sum2 || bad1 == sum3 || bad1 == sum4)
                {
                    bad2 = ran.Next(0, 25);
                    count++; if (count > 10) { bad1 = 25; break; }
                }
                count = 0;
                bad2 = ran.Next(0, 25);
                while (bad2 == bad1 || bad2 == sum1 || bad2 == sum2 || bad2 == sum3 || bad2 == sum4)
                {
                    bad2 = ran.Next(0, 25);
                    count++; if (count > 10) { bad2 = 25; break; }
                }
                count = 0;
                ReplayString = string.Format("{0}\r\n你的明日运势如下~\r\n宜：\r\n{1}\r\n{2}\r\n{3}\r\n{4}\r\n忌：\r\n{5}\r\n{6}\r\n今日日期：{7}",
                                            CQ.CQCode_At(fromQQ),
                                            GoodThings[sum1],
                                            GoodThings[sum2],
                                            GoodThings[sum3],
                                            GoodThings[sum4],
                                            BadThings[bad1],
                                            BadThings[bad2],
                                            System.DateTime.Today.ToString().Replace(" 0:00:00", "")
                                            );
                SendMinecraftMessage(fromGroup, ReplayString);
            }
            else if (msg.IndexOf("！addadmin ") == 0 && fromQQ == 961726194)
            {
                insert(123456, "给我列一下狗管理", msg.Replace("！addadmin ", ""));
                SendMinecraftMessage(fromGroup, "已添加一位狗管理");
            }
            else if (msg.IndexOf("！deladmin ") == 0 && fromQQ == 961726194)
            {
                remove(123456, "给我列一下狗管理", msg.Replace("！deladmin ", ""));
                SendMinecraftMessage(fromGroup, "已删除一位狗管理");
            }
            else if(msg == "给我列一下狗管理")
            {
                SendMinecraftMessage(fromGroup, "当前狗管理如下：\r\n" + list_get(123456, "给我列一下狗管理"));
            }
            else if(msg == "抽奖" && fromGroup != 241464054)
            {
                if (me.Authority == "成员")
                {
                    SendMinecraftMessage(fromGroup, "我没有禁言权限，无法使用抽奖功能");
                }
                else if(groupMember.Authority != "管理员" && groupMember.Authority != "群主")
                {
                    Random ran = new Random(System.DateTime.Now.Millisecond);
                    int RandKey = ran.Next(1, 22);
                    int RandKey2 = ran.Next(0, 10);
                    if (RandKey > 12)
                    {
                        SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "\r\n恭喜你！什么也没有抽中！");
                    }
                    else if (RandKey == 1 && RandKey2 != 0)
                    {
                        CQ.SetGroupMemberGag(fromGroup, fromQQ, RandKey * 3600 * 24 * 20);
                        SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "\r\n恭喜你抽中了超豪华禁言套餐，并附赠10张禁言卡！奖励已发放！");
                        int fk = 0;
                        string fks = xml_get(10, fromQQ.ToString());
                        if (fks != "")
                        {
                            fk = int.Parse(fks);
                        }
                        fk += 10;
                        del(10, fromQQ.ToString());
                        insert(10, fromQQ.ToString(), fk.ToString());
                    }
                    else if (RandKey == 1 && RandKey2 == 0)
                    {
                        CQ.SetGroupMemberGag(fromGroup, fromQQ, RandKey * 3600 * 24 * 30);
                        SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "\r\n恭喜你抽中了顶级豪华月卡禁言套餐，并附赠200张禁言卡！奖励已发放！");
                        int fk = 0;
                        string fks = xml_get(10, fromQQ.ToString());
                        if (fks != "")
                        {
                            fk = int.Parse(fks);
                        }
                        fk += 200;
                        del(10, fromQQ.ToString());
                        insert(10, fromQQ.ToString(), fk.ToString());
                    }
                    else if (RandKey < 11)
                    {
                        CQ.SetGroupMemberGag(fromGroup, fromQQ, RandKey * 3600 * 24);
                        SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "\r\n恭喜你抽中了禁言" + RandKey + "天！奖励已发放到你的QQ~");
                    }
                    else
                    {
                        int fk = 0;
                        string fks = xml_get(10, fromQQ.ToString());
                        if (fks != "")
                        {
                            fk = int.Parse(fks);
                        }
                        fk++;
                        SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "\r\n恭喜你抽中了一张禁言卡，回复“禁言卡”可以查看使用帮助。");
                        del(10, fromQQ.ToString());
                        insert(10, fromQQ.ToString(), fk.ToString());
                    }
                }
                else
                {
                    SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "\r\n你个管理员玩个鸡毛的抽奖啊，我又没法禁言你。");
                }
            }
            else if(msg == "禁言卡")
            {
                int fk = 0;
                string fks = xml_get(10, fromQQ.ToString());
                if (fks != "")
                {
                    fk = int.Parse(fks);
                }
                SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "\r\n禁言卡可用于禁言或解禁他人，如果接待权限足够。\r\n使用方法：发送禁言或解禁加上@那个人\r\n禁言时长将为1分钟-10分钟随机\r\n获取方式：抽奖时有十分之一的概率获得\r\n你当前剩余的禁言卡数量：" + fk.ToString());
            }
            else if (msg.IndexOf("禁言") == 0)
            {
                int fk = 0;
                string fks = xml_get(10, fromQQ.ToString());
                if (fks != "")
                {
                    fk = int.Parse(fks);
                }
                if (fk > 0)
                {
                    if ((groupMember.Authority != "管理员" && groupMember.Authority != "群主") || me.Authority == "群主" || fromGroup == 241464054)
                    {
                        try
                        {
                            long qq = GetNumberLong(msg);
                            var qqinfo = CQ.GetGroupMemberInfo(fromGroup, qq);
                            if (qqinfo.Authority != "管理员" || fromGroup == 241464054)
                            {
                                Random ran = new Random(System.DateTime.Now.Millisecond);
                                int RandKey = ran.Next(60, 600);
                                CQ.SetGroupMemberGag(fromGroup, qq, RandKey);
                                SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "\r\n已将" + qq + "禁言" + RandKey / 60 + "分钟");
                                fk--;
                                del(10, fromQQ.ToString());
                                insert(10, fromQQ.ToString(), fk.ToString());
                            }
                            else
                            {
                                SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "\r\n目标成员为管理员，我没法禁言那个辣鸡玩意儿");
                            }
                        }
                        catch
                        {
                            SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "\r\n执行失败。");
                        }
                    }
                    else
                    {
                        SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "\r\n你个管理员不会自己手动去禁言？智障？");
                    }
                }
                else
                {
                    SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "\r\n你哪儿有禁言卡？");
                }

            }
            else if (msg.IndexOf("解禁") == 0)
            {
                int fk = 0;
                string fks = xml_get(10, fromQQ.ToString());
                if (fks != "")
                {
                    fk = int.Parse(fks);
                }
                if (fk > 0)
                {
                    if ((groupMember.Authority != "管理员" && groupMember.Authority != "群主") || me.Authority == "群主" || fromGroup == 241464054)
                    {
                        try
                        {
                            long qq = GetNumberLong(msg);
                            var qqinfo = CQ.GetGroupMemberInfo(fromGroup, qq);
                            if (qqinfo.Authority != "管理员" || fromGroup == 241464054)
                            {
                                CQ.SetGroupMemberGag(fromGroup, qq, 0);
                                SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "\r\n已将" + qq + "解除禁言");
                                fk--;
                                del(10, fromQQ.ToString());
                                insert(10, fromQQ.ToString(), fk.ToString());
                            }
                            else
                            {
                                SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "\r\n目标成员为管理员，我没法解禁那个辣鸡玩意儿");
                            }
                        }
                        catch
                        {
                            SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "\r\n执行失败。");
                        }
                    }
                    else
                    {
                        SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "\r\n你个管理员不会自己手动去解禁？智障？");
                    }
                }
                else
                {
                    SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "\r\n你哪儿有禁言卡？");
                }

            }
            else if (msg.IndexOf("淘宝") == 0 && msg.Length > 2)
            {
                SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "你搜索的" + msg.Replace("淘宝", "") + "的相关优惠结果如下：\r\nhttps://ai.taobao.com/search/index.htm?key=" + System.Web.HttpUtility.UrlEncode(msg.Replace("淘宝", "")) + "&pid=mm_96609811_10528667_128948010");
            }
            else if(msg == "pixel")
            {
                int picCount;
                try
                {
                    picCount = int.Parse(xml_get(20, "count"));
                }
                catch
                {
                    SendMinecraftMessage(fromGroup, "遇到致命性错误，请联系晨旭修复");
                    return;
                }
                SendMinecraftMessage(fromGroup, "[CQ:image,file=pixel_game\\" + picCount + ".png]\r\n当前图片已被修改过" + picCount + "次。");
            }
            else if(msg.IndexOf("pixel") == 0 && (msg.Length - msg.Replace("/","").Length) == 2)
            {
                string fromqqtime_get = xml_get(20, fromQQ.ToString());
                int fromqqtime = 0;
                try
                {
                    fromqqtime = int.Parse(fromqqtime_get);
                }
                catch { }

                if(ConvertDateTimeInt(DateTime.Now)-fromqqtime < 60)
                {
                    SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "你需要再等" + (60 - ConvertDateTimeInt(DateTime.Now) + fromqqtime) + "秒才能继续放像素点");
                    return;
                }

                string get_msg = msg.Replace("pixel", ""), getx = "", gety = "", getcolor = "";
                int placex, placey;

                string[] str2;
                int count_temp = 0;
                str2 = get_msg.Split('/');
                foreach (string i in str2)
                {
                    if (count_temp == 0)
                    {
                        getx = i.ToString();
                        count_temp++;
                    }
                    else if (count_temp == 1)
                    {
                        gety = i.ToString();
                        count_temp++;
                    }
                    else if (count_temp == 2)
                    {
                        getcolor = i.ToString();
                        count_temp++;
                    }
                }
                try
                {
                    placex = int.Parse(getx) - 1;
                    placey = int.Parse(gety) - 1;
                    if (getcolor.IndexOf("#") == -1 || placex > 99 || placey > 99)
                        throw new ArgumentNullException("fuck wrong color");
                }
                catch
                {
                    SendMinecraftMessage(fromGroup, "放置像素点时遇到未知错误，请检查颜色与坐标是否正确");
                    return;
                }
                int picCount;
                try
                {
                    picCount = int.Parse(xml_get(20, "count"));
                }
                catch
                {
                    SendMinecraftMessage(fromGroup, "遇到致命性错误，请联系晨旭修复");
                    return;
                }

                try
                {
                    string picPath = @"C:\Users\Administrator\Desktop\kuqpro\data\image\pixel_game\" + picCount + ".png";
                    Bitmap pic = ReadImageFile(picPath);
                    pic = SetPoint(pic, ColorTranslator.FromHtml(getcolor), placex, placey);

                    picCount++;
                    picPath = @"C:\Users\Administrator\Desktop\kuqpro\data\image\pixel_game\" + picCount + ".png";
                    pic.Save(picPath);
                }
                catch
                {
                    SendMinecraftMessage(fromGroup, "遭遇未知错误");
                    return;
                }


                del(20, "count");
                del(20, fromQQ.ToString());
                insert(20, "count", picCount.ToString());
                insert(20, fromQQ.ToString(), ConvertDateTimeInt(DateTime.Now).ToString());

                //SendMinecraftMessage(fromGroup, "[CQ:image,file=pixel_game\\" + picCount + ".png]\r\n图片修改完成！" + DateTime.Now.ToString() + CQ.CQCode_At(fromQQ));
                SendMinecraftMessage(fromGroup, "图片修改完成！使用命令pixel查看" + CQ.CQCode_At(fromQQ));
            }
            else if(msg.IndexOf("查快递") == 0)
            {
                string kdcode = msg.Replace("查快递", "");

                if (msg== "查快递")
                {
                    kdcode = xml_get(9, fromQQ.ToString());
                    if(kdcode == "")
                    {
                        SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "你没有查询过任何快递，请输入要查询的单号");
                        return;
                    }
                }
                else
                {
                    del(9, fromQQ.ToString());
                    insert(9, fromQQ.ToString(), kdcode);
                }

                string result_msg = "";
                try
                {
                    string html = HttpGet("https://www.kuaidi100.com/autonumber/autoComNum", "text=" + kdcode);
                    JObject jo = (JObject)JsonConvert.DeserializeObject(html);
                    string comCode = jo["auto"][0]["comCode"].ToString();
                    result_msg = comCode + "\r\n";

                    html = HttpGet("https://www.kuaidi100.com/query", "type=" + comCode + "&postid=" + kdcode);
                    jo = (JObject)JsonConvert.DeserializeObject(html);
                    foreach (var i in jo["data"])
                    {
                        result_msg += i["time"].ToString() + " ";
                        result_msg += i["context"].ToString() + " 地点：";
                        result_msg += i["location"].ToString() + "\r\n";
                    }
                    if(result_msg == comCode + "\r\n")
                    {
                        result_msg = "";
                    }
                }
                catch
                {

                }
                if(result_msg=="")
                {
                    SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "无此单号的数据" + "\r\n下次查询该快递可直接发送“查快递”命令，无需在输入单号");
                }
                else
                {
                    SendMinecraftMessage(fromGroup, result_msg + "下次查询该快递可直接发送“查快递”命令，无需在输入单号\r\n" + CQ.CQCode_At(fromQQ));
                }
            }
            else if(msg.IndexOf("下载图片") == 0)
            {
                if(fromQQ != 961726194)
                {
                    SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "\r\n下你mb");
                    return;
                }
                bool Value = false;
                WebResponse response = null;
                Stream stream = null;

                string FileName = GetRandomString(10, true, false, false, false, "ABCDEF");

                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(msg.Replace("下载图片",""));

                    response = request.GetResponse();
                    stream = response.GetResponseStream();

                    if (!response.ContentType.ToLower().StartsWith("text/"))
                    {
                        Value = SaveBinaryFile(response, @"C:\Users\Administrator\Desktop\kuqpro\data\image\download\" + FileName);
                    }
                    SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "图片下载完成：\r\n[CQ:image,file=download\\" + FileName + "]");
                }
                catch (Exception err)
                {
                    string aa = err.Message.ToString();
                    SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "\r\n机器人爆炸了，原因：" + aa);
                }
            }
            else if(msg.IndexOf("网易云") == 0)
            {
                int songID = 0;
                try
                {
                    songID = int.Parse(msg.Replace("网易云", ""));
                }
                catch
                {
                    try
                    {
                        string html = HttpGet("http://s.music.163.com/search/get/", "type=1&s=" + msg.Replace("网易云", ""));
                        JObject jo = (JObject)JsonConvert.DeserializeObject(html);
                        string idGet = jo["result"]["songs"][0]["id"].ToString();
                        songID = int.Parse(idGet);
                    }
                    catch
                    {
                        SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "\r\n机器人爆炸了，原因：根本没这首歌");
                        return;
                    }
                    
                }

                bool Value = false;
                WebResponse response = null;
                Stream stream = null;

                string FileName = msg.Replace("网易云", "") + ".mp3";

                string url = HttpGet("https://www.chenxublog.com/163music/", "id=" + songID);

                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                    response = request.GetResponse();
                    stream = response.GetResponseStream();

                    if (!response.ContentType.ToLower().StartsWith("text/"))
                    {
                        Value = SaveBinaryFile(response, @"C:\Users\Administrator\Desktop\kuqpro\data\record\download\" + FileName);
                    }
                    SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "音乐加载完成，正在发送音乐，请稍后~");
                    SendMinecraftMessage(fromGroup, "[CQ:record,file=download\\" + FileName + "]");
                }
                catch (Exception err)
                {
                    string aa = err.Message.ToString();
                    SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "\r\n机器人爆炸了，原因：" + aa);
                }
            }
            else if (msg.IndexOf("找番号") == 0 || msg.IndexOf("查番号") == 0 || msg.IndexOf("查磁链") == 0 || msg.IndexOf("找磁链") == 0)
            {
                if (System.DateTime.Now.Hour > 5 && fromGroup != 115872123 && fromQQ != 961726194)
                {
                    SendMinecraftMessage(fromGroup, "开车时间为00:00-6:00");
                    return;
                }
                string url = msg;
                url = url.Replace("找番号", "");
                url = url.Replace("查番号", "");
                url = url.Replace("查磁链", "");
                url = url.Replace("找磁链", "");

                string html = HttpGet("http://torrentkitty.uno/tk/" + url + "/1-0-0.html", "?q=1");
                //Console.WriteLine(html);
                if (html == "")
                {
                    SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "加载失败");
                }
                else
                {
                    string magnets = "";
                    int i = 0, j = 0, k = 0;
                    MatchCollection matchs = Reg_solve(html, "<span>\\[(?<type>.*?)\\]&nbsp;</span><a href=(.*?)target=\"_blank\">(?<name>.*?)</a>");
                    string[] type = new string[matchs.Count];
                    string[] name = new string[matchs.Count];
                    MatchCollection check;
                    foreach (Match item in matchs)
                    {
                        if (item.Success)
                        {
                            string check_str = "";
                            check = Reg_solve(item.Groups["name"].Value.Replace("<b>", "").Replace("</b>", ""), "<span class=\"__cf_email__\" data-cfemail=\"(.*?)\">\\[email&#160;protected\\]</span>");
                            foreach (Match check_item in check)
                            {
                                check_str = check_item.Value;
                            }
                            type[i++] = item.Groups["type"].Value;
                            if (check_str != "")
                                name[j++] = item.Groups["name"].Value.Replace("<b>", "").Replace("</b>", "").Replace(check_str, "");
                            else
                                name[j++] = item.Groups["name"].Value.Replace("<b>", "").Replace("</b>", "");
                        }
                    }
                    MatchCollection link_matchs = Reg_solve(html, " href='magnet:\\?xt=urn:btih:(?<magnet>.*?)&dn=(.*?)' >磁力链接(.*?)<b>(?<filenum>\\d*?)个文件(.*?)共<b>(?<size>.*?)</b>");
                    i = 0;
                    j = 0;
                    string[] magnet = new string[link_matchs.Count];
                    string[] filenum = new string[link_matchs.Count];
                    string[] size = new string[link_matchs.Count];
                    foreach (Match item in link_matchs)
                    {
                        if (item.Success)
                        {
                            magnet[i++] = item.Groups["magnet"].Value;
                            filenum[j++] = item.Groups["filenum"].Value;
                            size[k++] = item.Groups["size"].Value;
                        }
                    }

                    for (i = 0; i < link_matchs.Count; i++)
                    {
                        try
                        {
                            magnets += "\r\n[" + type[i] + "]" + name[i] + "," + size[i] + "," + filenum[i] + "个文件\r\n" + magnet[i];
                        }
                        catch { }
                    }
                    SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "\r\n"+ url.Replace(" ", "-") + "的资源：" + magnets);
                }
            }
            else if(msg.IndexOf("*") == 0 && fromQQ == 961726194)
            {
                foreach(long i in grouplist)
                {
                    if(i != fromGroup)
                        SendMinecraftMessage(i, "跨群通知：\r\n" + msg.Substring(1));
                }
            }
            else if (msg == "开车")
            {
                SendMinecraftMessage(fromGroup, "假车：magnet:?xt=urn:btih:" + GetRandomString(40, true, false, false, false, "ABCDEF"));
            }
            else if(msg.IndexOf("正则") == 0)
            {
                string get_msg = msg.Replace("正则", "").Replace("\r",""), text = "", reg = "";

                if (get_msg.IndexOf("\n") >= 1 && get_msg.IndexOf("\n") != get_msg.Length - 1)
                {
                    string[] str2;
                    int count_temp = 0;
                    str2 = get_msg.Split('\n');
                    foreach (string i in str2)
                    {
                        if (count_temp == 0)
                        {
                            text = i.ToString();
                            count_temp++;
                        }
                        else if (count_temp == 1)
                        {
                            reg = i.ToString();
                            break;
                        }
                    }
                    string result_msg = "匹配如下：";
                    MatchCollection matchs = Reg_solve(text, reg);
                    foreach (Match item in matchs)
                    {
                        if (item.Success)
                        {
                            result_msg += "\r\n" + item.Value;
                        }
                    }
                    SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + result_msg);
                }
                else
                {
                    SendMinecraftMessage(fromGroup, "格式错误！");
                }

            }
            else if (replay_ok != "")
            {
                if (replay_common != "")
                {
                    Random ran = new Random(System.DateTime.Now.Millisecond);
                    int RandKey = ran.Next(0, 2);
                    if (RandKey == 0) { SendMinecraftMessage(fromGroup, replay_ok); } else { SendMinecraftMessage(fromGroup, replay_common); }
                }
                else
                {
                    SendMinecraftMessage(fromGroup, replay_ok);
                }
            }
            else if (replay_common != "")
            {
                SendMinecraftMessage(fromGroup, replay_common);
            }
        }

        public static MatchCollection Reg_solve(string str, string regstr)
        {
            Regex reg = new Regex(regstr, RegexOptions.IgnoreCase);
            return reg.Matches(str);
        }


        public static void ReplayGroupStatic(long fromGroup, string msg)
        {
            string replay_ok = replay_get(fromGroup, msg);
            string replay_common = replay_get(2333, msg);

            //CQ.SendGroupMessage(fromGroup, msg);

            //System.Windows.Forms.MessageBox.Show(msg);

            if (replay_ok != "")
            {
                if (replay_common != "")
                {
                    Random ran = new Random(System.DateTime.Now.Millisecond);
                    int RandKey = ran.Next(0, 2);
                    if (RandKey == 0) { SendMinecraftMessage(fromGroup, replay_ok); } else { SendMinecraftMessage(fromGroup, replay_common); }
                }
                else
                {
                    SendMinecraftMessage(fromGroup, replay_ok);
                }
            }
            else if (replay_common != "")
            {
                SendMinecraftMessage(fromGroup, replay_common);
            }
        }


        static string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;//AppDomain.CurrentDomain.SetupInformation.ApplicationBase


        public static string replay_get(long group, string msg)
        {
            dircheck(group);
            XElement root = XElement.Load(path + group + ".xml");
            string[] replay_str = new string[50];
            int count = 0;
            Random ran = new Random(System.DateTime.Now.Millisecond);
            int RandKey;
            string ansall = "";
            foreach (XElement mm in root.Elements("msginfo"))
            {
                if (msg.IndexOf(mm.Element("msg").Value) != -1 && count < 50)
                {
                    replay_str[count] = mm.Element("ans").Value;
                    count++;
                }
            }
            if (count != 0)
            {
                RandKey = ran.Next(0, count);
                ansall = replay_str[RandKey];
            }

            return ansall;
        }

        public static long qq_get(string msg)
        {
            long group = 1;
            dircheck(group);
            XElement root = XElement.Load(path + group + ".xml");
            string ansall = "";
            long ansqq = 0;
            foreach (XElement mm in root.Elements("msginfo"))
            {
                if (msg == mm.Element("ans").Value)
                {
                    ansall = mm.Element("msg").Value;
                    break;
                }
            }

            if(ansall != "")
            {
                ansqq = long.Parse(ansall);
            }

            return ansqq;
        }

        public static long qq_get_unregister(string msg)
        {
            long group = 5;
            dircheck(group);
            XElement root = XElement.Load(path + group + ".xml");
            string ansall = "";
            long ansqq = 0;
            foreach (XElement mm in root.Elements("msginfo"))
            {
                if (msg == mm.Element("ans").Value)
                {
                    ansall = mm.Element("msg").Value;
                    break;
                }
            }

            if (ansall != "")
            {
                ansqq = long.Parse(ansall);
            }

            return ansqq;
        }

        public static string xml_get(long group, string msg)
        {
            dircheck(group);
            XElement root = XElement.Load(path + group + ".xml");
            string ansall = "";
            foreach (XElement mm in root.Elements("msginfo"))
            {
                if (msg == mm.Element("msg").Value)
                {
                    ansall = mm.Element("ans").Value;
                    break;
                }
            }

            return ansall;
        }

        public static string list_get(long group, string msg)
        {
            dircheck(group);
            XElement root = XElement.Load(path + group + ".xml");
            int count = 0;
            string ansall = "";
            foreach (XElement mm in root.Elements("msginfo"))
            {
                if (msg == mm.Element("msg").Value)
                {
                    ansall = ansall + mm.Element("ans").Value + "\r\n";
                    count++;
                }
            }
            ansall = ansall + "一共有" + count.ToString() + "条回复";
            return ansall;
        }

        public static void del(long group, string msg)
        {
            dircheck(group);
            string gg = group.ToString();
            XElement root = XElement.Load(path + group + ".xml");

            var element = from ee in root.Elements()
                          where (string)ee.Element("msg") == msg
                          select ee;
            if (element.Count() > 0)
            {
                element.First().Remove();
            }
            root.Save(path + group + ".xml");
        }

        public static void remove(long group, string msg, string ans)
        {
            dircheck(group);
            string gg = group.ToString();
            XElement root = XElement.Load(path + group + ".xml");

            var element = from ee in root.Elements()
                          where (string)ee.Element("msg") == msg && (string)ee.Element("ans") == ans
                          select ee;
            if (element.Count() > 0)
            {
                element.First().Remove();
            }
            root.Save(path + group + ".xml");
        }


        public static void insert(long group, string msg, string ans)
        {
            if(msg.IndexOf("\r\n") < 0 & msg != "")
            {
                dircheck(group);
                XElement root = XElement.Load(path + group + ".xml");

                XElement read = root.Element("msginfo");

                read.AddBeforeSelf(new XElement("msginfo",
                       //new XElement("group", group),
                       new XElement("msg", msg),
                       new XElement("ans", ans)
                       ));

                root.Save(path + group + ".xml");
            }
        }

        public static void createxml(long group)
        {
            XElement root = new XElement("Categories",
                new XElement("msginfo",
                    //new XElement("group", 123),
                    new XElement("msg", "初始问题"),
                    new XElement("ans", "初始回答")
                    )
               );
            root.Save(path + group + ".xml");
        }

        public static void dircheck(long group)
        {
            if (File.Exists(path + group + ".xml"))
            {
                //MessageBox.Show("存在文件");
                //File.Delete(dddd);//删除该文件
            }
            else
            {
                //MessageBox.Show("不存在文件");
                createxml(group);//创建该文件，如果路径文件夹不存在，则报错。
            }
        }

        public static int AdminCheck(long fromQQ)
        {
            dircheck(123456);

            XElement root = XElement.Load(path +  "123456.xml");
            int count = 0;
            foreach (XElement mm in root.Elements("msginfo"))
            {
                if (mm.Element("ans").Value == fromQQ.ToString())
                {
                    count = 1;
                }
            }
            return count;
        }

        /// <summary>
        /// 获取字符串中的数字
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>数字</returns>
        public static long GetNumberLong(string str)
        {
            long result = 0;
            if (str != null && str != string.Empty)
            {
                // 正则表达式剔除非数字字符（不包含小数点.）
                str = Regex.Replace(str, @"[^\d.\d]", "");
                // 如果是数字，则转换为decimal类型
                if (Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$"))
                {
                    result = long.Parse(str);
                }
            }
            return result;
        }

        /// <summary>  
        /// GET 请求与获取结果  
        /// </summary>  
        public static string HttpGet(string Url, string postDataStr)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);

                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                return retString;
            }
            catch { }
            return "";
        }

        /// <summary>  
        /// GET 请求与获取结果（淘宝登陆） 
        /// </summary>  
        public static string HttpGetT(string Url, string postDataStr)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                //request.Headers.Add(":authority", "pub.alimama.com");
                //request.Headers.Add(":method", "GET");
                //request.Headers.Add(":path", (Url+"?"+postDataStr).Replace("https://pub.alimama.com",""));
                //request.Headers.Add(":scheme", "https");
                //request.Headers.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
                //request.Headers.Add("accept-language", "zh-CN,zh;q=0.8");
                //request.Headers.Add("cache-control", "max-age=0");
                request.Headers.Add("cookie", "t=73e5197b98069f33d6d4aad72b7a5346; cookie2=c4a9cdd990c418ed7a056a021ce6598c; v=0; _tb_token_=95PNEsOU8xq; cna=AxvTDzwMcFMCAduB7eMaxYyv; cookie32=b5e75e608a6d71a419548c89f962a775; cookie31=OTY2MDk4MTEsJUU2JTk5JUE4JUU2JTk3JUFEcTk2MTcyNjE5NCxsaXVjeDFAcXEuY29tLFRC; alimamapwag=TW96aWxsYS81LjAgKFdpbmRvd3MgTlQgNi4xOyBXT1c2NCkgQXBwbGVXZWJLaXQvNTM3LjM2IChLSFRNTCwgbGlrZSBHZWNrbykgQ2hyb21lLzU5LjAuMzA3MS4xMTUgU2FmYXJpLzUzNy4zNiBPUFIvNDYuMC4yNTk3LjU3; login=VT5L2FSpMGV7TQ%3D%3D; alimamapw=QHVXFSNxEidWEXZQQAwHVwVQUwZdBGgHAgdSAlFWBgVTUwVbV1QCAAIEAFBVVgFWAFMFV1YHBw%3D%3D; isg=Ai4udYVY-7qpiQ9eGGmryq_3b4Qwh5asb2XYvVj3mjHsO86VwL9COdQ7h5kr");
                //request.Headers.Add("dnt", "1");
                //request.Headers.Add("upgrade-insecure-requests", "1");
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36 OPR/46.0.2597.57";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);

                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                return retString;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            return "";
        }

        ///<summary>
        ///生成随机字符串 
        ///</summary>
        ///<param name="length">目标字符串的长度</param>
        ///<param name="useNum">是否包含数字，1=包含，默认为包含</param>
        ///<param name="useLow">是否包含小写字母，1=包含，默认为包含</param>
        ///<param name="useUpp">是否包含大写字母，1=包含，默认为包含</param>
        ///<param name="useSpe">是否包含特殊字符，1=包含，默认为不包含</param>
        ///<param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param>
        ///<returns>指定长度的随机字符串</returns>
        public static string GetRandomString(int length, bool useNum, bool useLow, bool useUpp, bool useSpe, string custom)
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;
            if (useNum == true) { str += "0123456789"; }
            if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }

        /// <summary>
        /// 改图片颜色
        /// </summary>
        /// <param name="Pict">图片</param>
        /// <param name="color">颜色</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        //x,y限制0-29
        private static Bitmap SetPoint(Bitmap Pict, Color color, int x, int y)
        {
            return SetPels(Pict, color, x * 17, y * 17, 17, 17);
        }

        public static Bitmap SetPels(Bitmap Pict, Color color, int x, int y, int w, int h)
        {
            //遍历矩形框内的各象素点
            for (int i = x; i < x + w; i++)
            {
                for (int j = y; j < y + h; j++)
                {
                    Pict.SetPixel(i, j, color);//设置当前象素点的颜色
                }
            }
            return Pict;
        }

        /// <summary>  
        /// DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time"> DateTime时间格式</param>  
        /// <returns>Unix时间戳格式</returns>  
        public static int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        /// <summary>
        /// 通过FileStream 来打开文件，这样就可以实现不锁定Image文件，到时可以让多用户同时访问Image文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Bitmap ReadImageFile(string path)
        {
            FileStream fs = File.OpenRead(path); //OpenRead
            int filelength = 0;
            filelength = (int)fs.Length; //获得文件长度 
            Byte[] image = new Byte[filelength]; //建立一个字节数组 
            fs.Read(image, 0, filelength); //按字节流读取 
            System.Drawing.Image result = System.Drawing.Image.FromStream(fs);
            fs.Close();
            Bitmap bit = new Bitmap(result);
            return bit;
        }


        /// <summary>
        /// Save a binary file to disk.
        /// </summary>
        /// <param name="response">The response used to save the file</param>
        // 将二进制文件保存到磁盘
        private static bool SaveBinaryFile(WebResponse response, string FileName)
        {
            bool Value = true;
            byte[] buffer = new byte[1024];

            try
            {
                if (File.Exists(FileName))
                    File.Delete(FileName);
                Stream outStream = System.IO.File.Create(FileName);
                Stream inStream = response.GetResponseStream();

                int l;
                do
                {
                    l = inStream.Read(buffer, 0, buffer.Length);
                    if (l > 0)
                        outStream.Write(buffer, 0, l);
                }
                while (l > 0);

                outStream.Close();
                inStream.Close();
            }
            catch
            {
                Value = false;
            }
            return Value;
        }


        private static string[] lunch =
        {
            "清炒鸡蛋",
            "现成的菜",
            "热干面",
            "小面",
            "烧茄子",
            "肉丝面",
            "拉面",
            "西红柿炒蛋",
            "鱼香茄子",
            "不吃"
        };
        private static string[] dinner =
        {
            "清炒鸡蛋",
            "现成的菜",
            "热干面",
            "小面",
            "烧茄子",
            "肉丝面",
            "拉面",
            "西红柿炒蛋",
            "鱼香茄子",
            "不吃",
            "好心人",
            "东门包子"
        };
        /// <summary>
        /// Type=4 讨论组消息。
        /// </summary>
        /// <param name="subType">子类型，目前固定为1。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromDiscuss">来源讨论组。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="msg">消息内容。</param>
        /// <param name="font">字体。</param>
        public override void DiscussMessage(int subType, int sendTime, long fromDiscuss, long fromQQ, string msg, int font)
        {
            // 处理讨论组消息。
            
            if (msg.IndexOf("今天吃") != -1 || msg.IndexOf("吃什么") != -1 || msg.IndexOf("吃啥") != -1)
            {
                Random ran = new Random(System.DateTime.Now.DayOfYear);
                int lunch_key = ran.Next(0, 10);
                int dinner_key = ran.Next(0, 12);
                CQ.SendDiscussMessage(fromDiscuss, String.Format("今天推荐你吃：\r\n午饭：{0}\r\n晚饭：{1}\r\n今日日期：{2}", 
                            lunch[lunch_key], dinner[dinner_key], System.DateTime.Today.ToString().Replace(" 0:00:00", "")));
            }

            /*
            if (msg.IndexOf("满楼") != -1)
            {
                CQ.SendDiscussMessage(fromDiscuss, "[CQ:record,file=CoolQ 语音时代！\\满楼\\2.mp3]");
            }*/


            string replay_common = replay_get(2333, msg);

            if (replay_common != "")
            {
                CQ.SendDiscussMessage(fromDiscuss, replay_common);
            }
        }

        /// <summary>
        /// Type=11 群文件上传事件。
        /// </summary>
        /// <param name="subType">子类型，目前固定为1。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromGroup">来源群号。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="file">上传文件信息。</param>
        public override void GroupUpload(int subType, int sendTime, long fromGroup, long fromQQ, string file)
        {
            // 处理群文件上传事件。
            //CQ.SendGroupMessage(fromGroup, String.Format("[{0}]{1}你上传了一个文件：{2}", CQ.ProxyType, CQ.CQCode_At(fromQQ), file));
            CQ.SendGroupMessage(fromGroup, String.Format("{1}上传了一个文件，会是什么东西呢？下载下来看看？", CQ.ProxyType, CQ.CQCode_At(fromQQ), file));
        }

        /// <summary>
        /// Type=101 群事件-管理员变动。
        /// </summary>
        /// <param name="subType">子类型，1/被取消管理员 2/被设置管理员。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromGroup">来源群号。</param>
        /// <param name="beingOperateQQ">被操作QQ。</param>
        public override void GroupAdmin(int subType, int sendTime, long fromGroup, long beingOperateQQ)
        {
            // 处理群事件-管理员变动。
            //CQ.SendGroupMessage(fromGroup, String.Format("[{0}]{2}({1})被{3}管理员权限。", CQ.ProxyType, beingOperateQQ, CQE.GetQQName(beingOperateQQ), subType == 1 ? "取消了" : "设置为"));
            CQ.SendGroupMessage(fromGroup, String.Format("恭喜{2}({1}){3}狗管理！", CQ.ProxyType, beingOperateQQ, CQE.GetQQName(beingOperateQQ), subType == 1 ? "摆脱了" : "变成了本群的"));
        }

        /// <summary>
        /// Type=102 群事件-群成员减少。
        /// </summary>
        /// <param name="subType">子类型，1/群员离开 2/群员被踢 3/自己(即登录号)被踢。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromGroup">来源群。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="beingOperateQQ">被操作QQ。</param>
        public override void GroupMemberDecrease(int subType, int sendTime, long fromGroup, long fromQQ, long beingOperateQQ)
        {
            // 处理群事件-群成员减少。
            //CQ.SendGroupMessage(fromGroup, String.Format("[{0}]群员{2}({1}){3}", CQ.ProxyType, beingOperateQQ, CQE.GetQQName(beingOperateQQ), subType == 1 ? "退群。" : String.Format("被{0}({1})踢除。", CQE.GetQQName(fromQQ), fromQQ)));
            string reply = xml_get(1, beingOperateQQ.ToString());
            if (fromGroup == 241464054 && reply != "")
            {
                del(1, beingOperateQQ.ToString());
                del(5, beingOperateQQ.ToString());
                CQ.SendGroupMessage(fromGroup, $"玩家{reply}（{beingOperateQQ}）的白名单已被删除。");
                mcmsg += "|||||command>manudel " + reply;
            }
            else
            {
                CQ.SendGroupMessage(fromGroup, String.Format("群员{2}({1}){3}", CQ.ProxyType, beingOperateQQ, CQE.GetQQName(beingOperateQQ), subType == 1 ? "因为精神失常离开了本群！" : String.Format("因为精神失常，被{0}({1})移出了本群！", CQE.GetQQName(fromQQ), fromQQ)));
            }
        }

        /// <summary>
        /// Type=103 群事件-群成员增加。
        /// </summary>
        /// <param name="subType">子类型，1/管理员已同意 2/管理员邀请。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromGroup">来源群。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="beingOperateQQ">被操作QQ。</param>
        public override void GroupMemberIncrease(int subType, int sendTime, long fromGroup, long fromQQ, long beingOperateQQ)
        {
            // 处理群事件-群成员增加。
            //CQ.SendGroupMessage(fromGroup, String.Format("[{0}]群里来了新人{2}({1})，管理员{3}({4}){5}", CQ.ProxyType, beingOperateQQ, CQE.GetQQName(beingOperateQQ), CQE.GetQQName(fromQQ), fromQQ, subType == 1 ? "同意。" : "邀请。"));
            if (fromGroup == 241464054)
            {
                CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + "\r\n欢迎加入本群！请在群里发送“绑定”+“你自己的id”来绑定（没空格），如：" +
                                                                        "\r\n绑定notch" +
                                                                        "\r\n未绑定id将不会进行白名单审核");
            }
        }

        /// <summary>
        /// Type=201 好友事件-好友已添加。
        /// </summary>
        /// <param name="subType">子类型，目前固定为1。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromQQ">来源QQ。</param>
        public override void FriendAdded(int subType, int sendTime, long fromQQ)
        {
            // 处理好友事件-好友已添加。
            //CQ.SendPrivateMessage(fromQQ, String.Format("[{0}]你好，我的朋友！", CQ.ProxyType));
        }

        /// <summary>
        /// Type=301 请求-好友添加。
        /// </summary>
        /// <param name="subType">子类型，目前固定为1。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="msg">附言。</param>
        /// <param name="responseFlag">反馈标识(处理请求用)。</param>
        public override void RequestAddFriend(int subType, int sendTime, long fromQQ, string msg, string responseFlag)
        {
            // 处理请求-好友添加。
            //CQ.SetFriendAddRequest(responseFlag, CQReactType.Allow, "新来的朋友");
        }

        /// <summary>
        /// Type=302 请求-群添加。
        /// </summary>
        /// <param name="subType">子类型，目前固定为1。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromGroup">来源群号。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="msg">附言。</param>
        /// <param name="responseFlag">反馈标识(处理请求用)。</param>
        public override void RequestAddGroup(int subType, int sendTime, long fromGroup, long fromQQ, string msg, string responseFlag)
        {
            // 处理请求-群添加。
            //CQ.SetGroupAddRequest(responseFlag, CQRequestType.GroupAdd, CQReactType.Allow, "新群友");
        }
    }
}
