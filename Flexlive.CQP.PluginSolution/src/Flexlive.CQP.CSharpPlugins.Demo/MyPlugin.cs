using Flexlive.CQP.Framework;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
                                CQ.SendGroupMessage(241464054, i);
                                ReplayGroupStatic(241464054, i);
                            }
                        }
                        catch { }
                    }
                    else
                    {
                        CQ.SendGroupMessage(241464054, replay);
                        ReplayGroupStatic(241464054, replay);
                    }
                }
                clientSocket.Send(Encoding.UTF8.GetBytes("ok233"));
                clientSocket.Send(Encoding.UTF8.GetBytes(mcmsg));
                mcmsg = "";

                myClientSocket.Shutdown(SocketShutdown.Both);
                myClientSocket.Close();
            }
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
            else if (msg.IndexOf("绑定") != -1)
            {
                if (replay_get(1, fromQQ.ToString()) == "")
                {
                    insert(1, fromQQ.ToString(), msg.Replace("绑定", ""));
                    CQ.SendPrivateMessage(fromQQ, "绑定id:" + msg.Replace("绑定", "") + "成功！");
                }
                else
                {
                    CQ.SendPrivateMessage(fromQQ, "你已经绑定过了，想换id私聊服主去吧");
                }
            }
            else
            {
                CQ.SendPrivateMessage(fromQQ, "人家不认识你了啦");
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

            // 处理群消息。

            if (fromGroup == 241464054)
            {
                string reply = replay_get(1, fromQQ.ToString());
                if (reply != "")
                {
                    mcmsg += "|||||[群消息]<" + reply + ">" + msg;
                }
                else
                {
                    CQ.SendPrivateMessage(fromQQ, "检测到你没有绑定服务器id，请回复“绑定id”来绑定（没空格），如：\r\n绑定notch");
                }
            }


            //if (fromGroup == 339837275)
            //{
            var groupMember = CQ.GetGroupMemberInfo(fromGroup, fromQQ);

            //CQ.SendGroupMessage(fromGroup, String.Format("[{4}]{0} 你的群名片：{1}， 入群时间：{2}， 最后发言：{3}。", CQ.CQCode_At(fromQQ),
            //    groupMember.GroupCard, groupMember.JoinTime, groupMember.LastSpeakingTime, CQ.ProxyType));
            // CQ.SendGroupMessage(fromGroup, String.Format("[{0}]{1}你发的群消息是：{2}", CQ.ProxyType, CQ.CQCode_At(fromQQ), msg));
            //CQ.SendGroupMessage(fromGroup, string.Format("{0}发的群消息是：{1}", CQ.CQCode_At(fromQQ), msg));
            string replay_ok = replay_get(fromGroup, msg);
            string replay_common = replay_get(2333, msg);

            //CQ.SendGroupMessage(fromGroup, msg);

            //System.Windows.Forms.MessageBox.Show(msg);

            if (msg.ToUpper() == "HELP")
            {
                //CQ.SendGroupMessage(fromGroup, msg.ToUpper().IndexOf("help").ToString());
                SendMinecraftMessage(fromGroup, "命令帮助：\r\n！add 词条：回答\r\n！del 词条：回答\r\n！list 词条\r\n所有符号均为全角符号\r\n词条中请勿包含冒号\r\n点歌功能测试中，关键词：点歌、坷垃金曲\r\n私聊发送“赞我”可使接待给你点赞\r\n发送“今日运势”可以查看今日运势\r\n如有bug请反馈");
            }
            else if (msg == "点歌" || msg == "坷垃金曲")
            {
                SendMinecraftMessage(fromGroup, string.Format("{0}正在发送歌曲，请稍候哦~", CQ.CQCode_At(fromQQ)));
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
            //else if (msg == "签到")
            //{
            //    Random ran = new Random(System.DateTime.Now.Millisecond);
            //    int RandKey = ran.Next(0, 500);
            //    CQ.SendGroupMessage(fromGroup, string.Format("{0}\r\n签到成功\r\n获得经验{1}点\r\n然而这并没有什么乱用\r\n所以本机器人并没有记录这条签到信息\r\n私聊发送“赞我”可给你点赞",
            //                                                    CQ.CQCode_At(fromQQ),
            //                                                    RandKey.ToString()
            //                                                    ));
            //}
            else if(msg == "今日黄历" || msg == "今日运势" || msg == "签到")
            {
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
                ReplayString = string.Format("{0}\r\n你的今日运势如下~\r\n宜：\r\n{1}\r\n{2}\r\n{3}\r\n{4}\r\n忌：\r\n{5}\r\n{6}\r\n今日日期：{7}",
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

            else if (msg == "昨日黄历" || msg == "昨日运势")
            {
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

            //}
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
        /*
        public static string ba_nrandom(long group, long qq)
        {
            SetGroupMemberGag(group, qq, 60);
            return "";
        }
        */

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
        /// GET 请求与获取结果  
        /// </summary>  
        public static string HttpGet(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            try
            {
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                return retString;
            }
            catch { }
            return "";
        }

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
            //CQ.SendDiscussMessage(fromDiscuss, String.Format("[{0}]{1}你发的讨论组消息是：{2}", CQ.ProxyType, CQ.CQCode_At(fromQQ), msg));
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
            //CQ.SetGroupAddRequest(responseFlag, CQRequestType.GroupAdd, CQReactType.Allow, "新群友");
        }
    }
}
