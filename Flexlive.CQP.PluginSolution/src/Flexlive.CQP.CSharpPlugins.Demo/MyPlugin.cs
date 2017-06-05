using Flexlive.CQP.Framework;
using System;
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
                                            Random ran = new Random(System.DateTime.Now.Millisecond);
                                            int RandKey = ran.Next(0, 501);
                                            CoinsTemp += RandKey;
                                            SendMinecraftMessage(241464054, CQ.CQCode_At(fromQQ) + "\r\n签到成功！获得游戏币" + RandKey + "枚！\r\n银行内游戏币" + CoinsTemp + "枚\r\n抽奖次数已重置为五次！\r\n回复“帮助”查看如何取钱");
                                            del(2, fromQQ.ToString());
                                            del(3, fromQQ.ToString());
                                            del(4, fromQQ.ToString());
                                            insert(2, fromQQ.ToString(), CoinsTemp.ToString());
                                            insert(3, fromQQ.ToString(), System.DateTime.Today.ToString());
                                            insert(4, fromQQ.ToString(), "0");
                                        }
                                    }
                                    else
                                    {
                                        //SendMinecraftMessage(241464054, "玩家" + i.Replace("<qd>", "") + "请到群241464054绑定自己的id！");
                                        mcmsg += "|||||command>kick " + i.Replace("<qd>", "") + " 请到群241464054绑定自己的id！";
                                    }
                                    
                                }
                                else
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
                        else
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
                                            "看不可描述的漫画：释放压力重铸自我",
                                            "敲代码/做视频/画画：一次编译通关/完美特效",
                                            "十连抽：一堆稀有",
                                            "在群里发红包：抢到别人发的一堆红包",
                                            "和女神聊天：从陌生人变成备胎",
                                            "不可描述：不可描述",
                                            "啦啦人/osu：各种full combo",
                                            "锻炼：八分钟给你比利般的身材",
                                            "散步：遇到妹子主动搭讪",
                                            "打排位赛：今天能赢一次了",
                                            "去上班：被夸奖工作认真",
                                            "抚摸猫咪：我是巧克力啊，你不认识我了吗？",
                                            "打扫：干干净净好心情",
                                            "烹饪：米饭是菜",
                                            "告白：原来你也是基佬",
                                            "xx神社：最新种子入手",
                                            "追新番：完结之前我绝不会死",
                                            "使用膜法：提高姿势水平",
                                            "深呼吸：长寿的秘诀是保持呼吸",
                                            "steam：-90%",
                                            "x宝x东：商品大减价",
                                            "买彩票：中了十块",
                                            "读书：知识就是力量",
                                            "早睡：早睡早起方能养生",
                                            "逛街：物美价廉大优惠",
                                            "迷の空缺位"};
        private static string[] BadThings = {
                                            "看不可描述的漫画：会被家人撞到",
                                            "敲代码/做视频/画画：各种bug/还没保存软件就无响应",
                                            "十连抽：全都是辣鸡",
                                            "在群里扯淡：浪费一整天",
                                            "和女神聊天：我去洗澡了，呵呵",
                                            "不可描述：不可描述",
                                            "啦啦人/osu：卡机全部miss",
                                            "锻炼：会拉伤肌肉",
                                            "散步：走路会踩到水坑",
                                            "打排位赛：我方三人挂机",
                                            "去上班：上班偷玩游戏被扣工资",
                                            "抚摸猫咪：喵嗷！（哗啦一道子",
                                            "打扫：会停水",
                                            "烹饪：难道这就是……仰望星空派？",
                                            "告白：对不起，你是一个好人",
                                            "xx神社：全都不和口味",
                                            "追新番：会被剧透",
                                            "使用膜法：上台拿衣服",
                                            "深呼吸：外面全是雾霾",
                                            "steam：卖的比TGP上的还贵",
                                            "x宝x东：问题产品需要退换",
                                            "买彩票：就差一点啊",
                                            "读书：注意力完全无法集中",
                                            "早睡：会在半夜醒来，然后失眠",
                                            "逛街：会遇到奸商",
                                            "迷の空缺位"};

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
                SendMinecraftMessage(241464054, "整点发钱！");
                Random ran = new Random(System.DateTime.Now.Millisecond);
                mcmsg += "|||||command>eco give * " + ran.Next(0, 200);
            }

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
            /*
            if(msg == "启动")
            {
                CQ.SendPrivateMessage(961726194, "扫描监考安排任务已启动，扫描每十秒执行一次，输入“结束”停止扫描，发送“查看”获取实时情况");
                CQ.SendPrivateMessage(1371773817, "扫描监考安排任务已启动，扫描每十秒执行一次，输入“结束”停止扫描，发送“查看”获取实时情况");
                isOpenScan = 1;
            }
            else if (msg == "结束")
            {
                CQ.SendPrivateMessage(961726194, "扫描监考安排任务已停止，输入“启动”启动扫描扫描每十秒执行一次，发送“查看”获取实时情况");
                CQ.SendPrivateMessage(1371773817, "扫描监考安排任务已停止，输入“启动”启动扫描，发送“查看”获取实时情况");
                isOpenScan = 0;
            }
            else if (msg == "查看")
            {
                string httpresult = HttpGet("http://wfstc.hpu.edu.cn/list.aspx?id=56", "");
                if (httpresult.IndexOf("第十五周考试监考安排") >= 0)
                {
                    CQ.SendPrivateMessage(961726194, "第十五周考试监考安排已发布，请尽快查看：http://wfstc.hpu.edu.cn/list.aspx?id=56\r\n当前任务开启状态：" + isOpenScan.ToString() );
                    CQ.SendPrivateMessage(1371773817, "第十五周考试监考安排已发布，请尽快查看：http://wfstc.hpu.edu.cn/list.aspx?id=56\r\n当前任务开启状态：" + isOpenScan.ToString() );
                }
                else if(httpresult == "")
                {
                    CQ.SendPrivateMessage(961726194, "加载失败鸟~~\r\n当前任务开启状态：" + isOpenScan.ToString());
                    CQ.SendPrivateMessage(1371773817, "加载失败鸟~~\r\n当前任务开启状态：" + isOpenScan.ToString());
                }
                else
                {
                    CQ.SendPrivateMessage(961726194, "第十五周考试监考安排还没发布\r\n当前任务开启状态：" + isOpenScan.ToString() );
                    CQ.SendPrivateMessage(1371773817, "第十五周考试监考安排还没发布\r\n当前任务开启状态：" + isOpenScan.ToString() );
                }
            }
            else */if (msg.ToUpper() == "赞我")
            {
                CQ.SendPraise(fromQQ, 10);
                CQ.SendPrivateMessage(fromQQ, "已为你一次性点赞十次，每天只能十次哦");
                //CQ.SendGroupMessage(fromGroup, "已为QQ" + fromQQ + "点赞十次");
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
                else if(msg.IndexOf("人数") != -1 || msg.IndexOf("少人") != -1 || msg.IndexOf("谁在") != -1 || msg.IndexOf("有谁") != -1 || msg.IndexOf("在线") != -1)
                {
                    mcmsg += "|||||sum>我要看人数";
                    mcmsg += "|||||[群消息]<" + reply + ">" + msg;
                }
                else if (reply != "")
                {
                    mcmsg += "|||||[群消息]<" + reply + ">" + msg;
                    //if (groupMember.GroupCard.IndexOf(reply) == -1)
                    //{
                        CQ.SetGroupNickName(fromGroup, fromQQ, reply);
                    //}
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
                                                        "\r\n" + CQ.CQCode_At(1021479600) + CQ.CQCode_At(635309406) + CQ.CQCode_At(1928361196) + CQ.CQCode_At(1420355171) + CQ.CQCode_At(280585112) + CQ.CQCode_At(2561620740) + CQ.CQCode_At(2433380978) + CQ.CQCode_At(2679146075) + CQ.CQCode_At(961726194));
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
                else if (msg == "催促审核")
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
                                                "\r\n" + CQ.CQCode_At(1021479600) + CQ.CQCode_At(635309406) +
                                                CQ.CQCode_At(1928361196) + CQ.CQCode_At(1420355171) + CQ.CQCode_At(280585112) +
                                                CQ.CQCode_At(2561620740) + CQ.CQCode_At(2433380978) + CQ.CQCode_At(2679146075) +
                                                CQ.CQCode_At(961726194));
                    SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "已成功催促管理员审核！请耐心等待！如果还没有被审核，你可以选择继续催促！");
                }
                else
                {
                    CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) + "\r\n检测到你没有绑定服务器id，请在群里发送“绑定”+“你自己的id”来绑定（没空格），如：\r\n绑定notch\r\n长时间未绑定你将会被移出本群");
                    CQ.SendGroupMessage(567145439, "接待喵糖拌管理：\r\nQQ：" + fromQQ.ToString() +
                                                "\r\n群名片：" + groupMember.GroupCard +
                                                "\r\n没有绑定id\r\n如长时间没绑请将其移出群");
                }

                if (msg == "签到")
                {
                    SendMinecraftMessage(fromGroup, "登陆游戏即可签到~");
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
                else if (msg.IndexOf("存钱") == 0)
                {
                    SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "无效命令，回复帮助查看更多。");
                }
                if (msg == "帮助")
                {
                    SendMinecraftMessage(fromGroup, "关于签到奖励：\r\n每日可随机领取0-500游戏币的奖励，奖励暂存在银行中。\r\n取钱方式：在群里发送“取钱”即可取出所有钱。\r\n回复存钱+金额可将服务器钱存入银行\r\n目前仅限“存钱100”、“存钱500”、“存钱1000”三个命令\r\n回复“查询”可查询当前银行内余额。\r\n回复“抽奖”可花费100游戏币进行抽奖，中奖率为玄学");
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

                        CoinsTemp1 += 2000;
                        CoinsTemp2 += 500;
                        del(2, fromQQ.ToString());
                        del(2, targetQQ.ToString());
                        insert(2, fromQQ.ToString(), CoinsTemp2.ToString());
                        insert(2, targetQQ.ToString(), CoinsTemp1.ToString());
                        insert(6, fromQQ.ToString(), "推荐人：" + targetQQ.ToString());
                        SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "游戏币奖励已发放到群内银行~快去告诉你的小伙伴吧~");
                    }
                    catch
                    {
                        SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "出现未知错误");
                    }
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
                SendMinecraftMessage(fromGroup, "命令帮助：\r\n！add 词条：回答\r\n！del 词条：回答\r\n！list 词条\r\n所有符号均为全角符号\r\n词条中请勿包含冒号\r\n发送“点歌”+数字序号即可点歌（如点歌14，最大134）\r\n发送“坷垃金曲”+数字序号即可点金坷垃歌（如坷垃金曲21，最大71）\r\n私聊发送“赞我”可使接待给你点赞\r\n发送“今日运势”可以查看今日运势\r\n如有bug请反馈");
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
                ReplayString = string.Format("{0}\r\n你的今日运势如下~\r\n宜：\r\n{1}\r\n{2}\r\n{3}\r\n{4}\r\n忌：\r\n{5}\r\n{6}\r\n今日日期：{7}\r\n今日综合幸运指数：{8}%",
                                            CQ.CQCode_At(fromQQ),
                                            GoodThings[sum1],
                                            GoodThings[sum2],
                                            GoodThings[sum3],
                                            GoodThings[sum4],
                                            BadThings[bad1],
                                            BadThings[bad2],
                                            System.DateTime.Today.ToString().Replace(" 0:00:00", ""),
                                            allsum.ToString()
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
                        CQ.SetGroupMemberGag(fromGroup, fromQQ, RandKey * 3600 * 7 + 2333);
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
                        CQ.SetGroupMemberGag(fromGroup, fromQQ, RandKey * 60);
                        SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "\r\n恭喜你抽中了禁言" + RandKey + "分钟！奖励已发放到你的QQ~");
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
                    if ((groupMember.Authority != "管理员" && groupMember.Authority != "群主") || me.Authority == "群主")
                    {
                        try
                        {
                            long qq = GetNumberLong(msg);
                            var qqinfo = CQ.GetGroupMemberInfo(fromGroup, qq);
                            if (qqinfo.Authority != "管理员")
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
                    if ((groupMember.Authority != "管理员" && groupMember.Authority != "群主") || me.Authority == "群主")
                    {
                        try
                        {
                            long qq = GetNumberLong(msg);
                            var qqinfo = CQ.GetGroupMemberInfo(fromGroup, qq);
                            if (qqinfo.Authority != "管理员")
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

            if (msg.IndexOf("找番号") == 0 || msg.IndexOf("查番号") == 0 || msg.IndexOf("查磁链") == 0 || msg.IndexOf("找磁链") == 0)
            {
                string url = msg;
                url = msg.Replace("找番号", "");
                url = msg.Replace("查番号", "");
                string html = HttpGet("http://www.cilisou.cn/s.php", "q=" + url.Replace(" ", "-"));
                if (html == "")
                {
                    SendMinecraftMessage(fromGroup, "加载失败");
                }
                else
                {
                    string magnets = "";
                    Regex reg = new Regex("magnet:\\?xt=urn:btih:(.*)\" target=\"_blank\"", RegexOptions.IgnoreCase);
                    MatchCollection matchs = reg.Matches(html);
                    foreach (Match item in matchs)
                    {
                        if (item.Success)
                        {
                                magnets += "\r\n" + item.Value.Replace("magnet:\\?xt=urn:btih:", "").Replace("\" target=\"_blank\"", "");
                        }
                    }
                    SendMinecraftMessage(fromGroup, CQ.CQCode_At(fromQQ) + "为你找到以下磁链：" + magnets);
                }
            }

            if(fromGroup== 115872123 && msg.IndexOf("跨群") == 0)
            {
                SendMinecraftMessage(567477706, groupMember.GroupCard + "(" + fromQQ + ")\r\n" + msg.Replace("跨群", "") + "\r\n消息来自某里世界，以“跨群”开头的消息可跨群发送");
            }
            if (fromGroup == 567477706 && msg.IndexOf("跨群") == 0)
            {
                SendMinecraftMessage(115872123, groupMember.GroupCard + "(" + fromQQ + ")\r\n" + msg.Replace("跨群", "") + "\r\n消息来自某崩坏群，以“跨群”开头的消息可跨群发送");
            }

            if (msg == "开车")
            {
                SendMinecraftMessage(fromGroup, "假车：magnet:?xt=urn:btih:" + GetRandomString(40, true, false, false, false, "ABCDEF"));
            }
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
                CQ.SendDiscussMessage(fromDiscuss, String.Format("今天推荐你吃：\r\n午饭：{0}\r\n晚饭：{1}\r\n今日日期：{2}", lunch[lunch_key], dinner[dinner_key], System.DateTime.Today.ToString().Replace(" 0:00:00", "")));
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
            CQ.SendGroupMessage(fromGroup, String.Format("群员{2}({1}){3}", CQ.ProxyType, beingOperateQQ, CQE.GetQQName(beingOperateQQ), subType == 1 ? "因为精神失常离开了本群！" : String.Format("因为精神失常，被{0}({1})移出了本群！", CQE.GetQQName(fromQQ), fromQQ)));
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
            if (fromGroup == 241464054)
            {
                CQ.SetGroupAddRequest(responseFlag, CQRequestType.GroupAdd, CQReactType.Allow, "新群友");
                CQ.SendGroupMessage(fromGroup, CQ.CQCode_At(fromQQ) +   "\r\n欢迎加入本群！请在群里发送“绑定”+“你自己的id”来绑定（没空格），如：" +
                                                                        "\r\n绑定notch" +
                                                                        "\r\n未绑定id将不会进行白名单审核");
            }
        }
    }
}
