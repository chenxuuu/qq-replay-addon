package com.sweetcreeper.qqplugin;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.PrintWriter;
import java.net.Socket;
import java.net.UnknownHostException;

import org.bukkit.Bukkit;
import org.bukkit.OfflinePlayer;
import org.bukkit.entity.Player;
import org.bukkit.event.EventHandler;
import org.bukkit.event.Listener;
import org.bukkit.event.entity.PlayerDeathEvent;
import org.bukkit.event.player.AsyncPlayerChatEvent;
import org.bukkit.event.player.PlayerJoinEvent;
import org.bukkit.event.player.PlayerQuitEvent;
import org.bukkit.plugin.RegisteredServiceProvider;
import org.bukkit.plugin.java.JavaPlugin;
import org.bukkit.scheduler.BukkitRunnable;

import net.milkbowl.vault.economy.Economy;


public class Qqplugin extends JavaPlugin implements Listener
{
	public String player="none233";
	public String msg="none233";
    public boolean isEco = false;
    public Economy economy;	
	
	
	@Override//��д����ķ���
	public void onEnable()
	{
		getLogger().info("QQGroupMessagePlugin is started successfully!");
		
		player="ok";
		msg="<��ʾ>��������������ɡ�";
		//ע�����
		Bukkit.getPluginManager().registerEvents(this,this);
		
		new BukkitRunnable(){     
		    int s = 0;//���ö�10���ִ��ĳ�δ���
		    @Override    
		    public void run(){
		    	if(s>=60)
		    	{
		    		Bukkit.getServer().dispatchCommand(Bukkit.getServer().getConsoleSender(), "tm abc ��Ǯ����");
		    		Bukkit.getServer().dispatchCommand(Bukkit.getServer().getConsoleSender(), "eco give * 1");
		    		s=0;
		    	}
		    	else
		    	{
		    		s++;
		    	}
		        //s--;//�����ݼ�,�ҿ��ٷ��Ľ̳���û�����,��û�Թ�,��Ҳ����ɾ������
		        //if(s==0){
		            //���д10���ִ�еĴ���(���綨��Ķ�ʱ��ÿ����1��)
		        //    cancel();//cancel����ȡ����ʱ��
		        //}else{
		            //�������дÿ�δ�����ʱ��ִ�еĴ���
		            try 
		            {
		                //1.�����ͻ���Socket��ָ����������ַ�Ͷ˿�
		                Socket socket=new Socket("localhost", 2333);
		                //2.��ȡ���������������˷�����Ϣ
		                OutputStream os=socket.getOutputStream();//�ֽ������
		                PrintWriter pw=new PrintWriter(os);//���������װΪ��ӡ��
		                if(player!="none233")
		                {
		                	pw.write(msg);
		                	player="none233";
		                }
		                else
		                {
		                	pw.write("getmsg");
		                }
		                pw.flush();
		                socket.shutdownOutput();//�ر������
		                //3.��ȡ������������ȡ�������˵���Ӧ��Ϣ
		                InputStream is=socket.getInputStream();
		                BufferedReader br=new BufferedReader(new InputStreamReader(is));
		                String info=null;
		                info=br.readLine();
		                String[] sourceStrArray=info.split("\\|\\|\\|\\|\\|");
		                for(int i=0;i<sourceStrArray.length;i++)
		                {
		                	if(sourceStrArray[i].indexOf("<")!=-1)
		                	{
		                		Bukkit.broadcastMessage(sourceStrArray[i]);
		                	}
		                	else if(sourceStrArray[i].indexOf("command>")!=-1)
		                	{
		                		Bukkit.getServer().dispatchCommand(Bukkit.getServer().getConsoleSender(), sourceStrArray[i].replace("command>", ""));
		                		if(player!="none233")
		                		{
		                			msg+="]][[<��ʾ>"+sourceStrArray[i].replace("command>", "")+"��ִ��";
		                		}
		                		else
		                		{
		                			player="ok";
		                			msg="<��ʾ>"+sourceStrArray[i].replace("command>", "")+"��ִ��";
		                		}
		                	}
		                	else if(sourceStrArray[i].indexOf("sum>")!=-1)
		                	{
		                		String result="";
		                		for(Player p : Bukkit.getOnlinePlayers())
		                		{
		                			result+=p.getName()+",";
		                		}
		                		if(player!="none233")
		                		{
		                			msg+="]][[<��ʾ>��������ǰ��������Ϊ"+Bukkit.getOnlinePlayers().size()+"�ˣ�����б�"+result;
		                		}
		                		else
		                		{
		                			player="ok";
		                			msg="<��ʾ>��������ǰ��������Ϊ"+Bukkit.getOnlinePlayers().size()+"�ˣ�����б�"+result;
		                		}
		                	}
		                	else if(sourceStrArray[i].indexOf("eco>")!=-1)
		                	{
		                		String result="",playername=sourceStrArray[i].replace("eco>", "");
		                        isEco = setupEconomy();
		                        if(isEco)
		                        {
		                        	@SuppressWarnings("deprecation")
									OfflinePlayer p = Bukkit.getOfflinePlayer(playername);
		                            //��ȡ��ҽ�Ǯ
		                        	if(p!=null)
		                        		result = "���"+ playername +"���������Ϊ��" + economy.getBalance(p);
		                        }
		                        else
		                        {
		                            //valutûװ����ess����ûװ
		                        	result="���"+ playername +"��������ѯ���ʧ�ܡ�";
		                        }
		                		if(player!="none233")
		                		{
		                			msg+="]][[<��ʾ>"+result;
		                		}
		                		else
		                		{
		                			player="ok";
		                			msg="<��ʾ>"+result;
		                		}
		                	}
		                	else if(sourceStrArray[i].indexOf("ecodel100>")!=-1)
		                	{
		                		String result="",playername=sourceStrArray[i].replace("ecodel100>", "");
		                        isEco = setupEconomy();
		                        if(isEco)
		                        {
		                        	@SuppressWarnings("deprecation")
									OfflinePlayer p = Bukkit.getOfflinePlayer(playername);
		                            if(economy.has(p,100) && p!=null)//�ж�����Ƿ�100Ԫ
		                            {
		                            	economy.withdrawPlayer(p,100);//�۳�100Ԫ
		                            	result = "<eco100>" + playername;
		                            }
		                            else
		                            {
		                            	result = playername + "����������Ǯ����100��";
		                            }
		                        }
		                        else
		                        {
		                            //valutûװ����ess����ûװ
		                        	result="���"+ playername +"��������ѯ���ʧ�ܡ�";
		                        }
		                		if(player!="none233")
		                		{
		                			msg+="]][["+result;
		                		}
		                		else
		                		{
		                			player="ok";
		                			msg=result;
		                		}
		                	}
		                	else if(sourceStrArray[i].indexOf("ecodel500>")!=-1)
		                	{
		                		String result="",playername=sourceStrArray[i].replace("ecodel500>", "");
		                        isEco = setupEconomy();
		                        if(isEco)
		                        {
		                        	@SuppressWarnings("deprecation")
									OfflinePlayer p = Bukkit.getOfflinePlayer(playername);
		                            if(economy.has(p,500) && p!=null)//�ж�����Ƿ�500Ԫ
		                            {
		                            	economy.withdrawPlayer(p,500);//�۳�500Ԫ
		                            	result = "<eco500>" + playername;
		                            }
		                            else
		                            {
		                            	result = playername + "����������Ǯ����500��";
		                            }
		                        }
		                        else
		                        {
		                            //valutûװ����ess����ûװ
		                        	result="���"+ playername +"��������ѯ���ʧ�ܡ�";
		                        }
		                		if(player!="none233")
		                		{
		                			msg+="]][["+result;
		                		}
		                		else
		                		{
		                			player="ok";
		                			msg=result;
		                		}
		                	}
		                	else if(sourceStrArray[i].indexOf("ecodel1000>")!=-1)
		                	{
		                		String result="",playername=sourceStrArray[i].replace("ecodel1000>", "");
		                        isEco = setupEconomy();
		                        if(isEco)
		                        {
		                        	@SuppressWarnings("deprecation")
									OfflinePlayer p = Bukkit.getOfflinePlayer(playername);
		                            if(economy.has(p,1000) && p!=null)//�ж�����Ƿ�1000Ԫ
		                            {
		                            	economy.withdrawPlayer(p,1000);//�۳�1000Ԫ
		                            	result = "<eco1000>" + playername;
		                            }
		                            else
		                            {
		                            	result = playername + "����������Ǯ����1000��";
		                            }
		                        }
		                        else
		                        {
		                            //valutûװ����ess����ûװ
		                        	result="���"+ playername +"��������ѯ���ʧ�ܡ�";
		                        }
		                		if(player!="none233")
		                		{
		                			msg+="]][["+result;
		                		}
		                		else
		                		{
		                			player="ok";
		                			msg=result;
		                		}
		                	}
		                }
		                //Bukkit.broadcastMessage("debug:"+info);
		                //4.�ر���Դ
		                br.close();
		                is.close();
		                pw.close();
		                os.close();
		                socket.close();
		            } catch (UnknownHostException e) {
		                //e.printStackTrace();
		            } catch (IOException e) {
		                //e.printStackTrace();
		            }
		        //}
		    } 
		}.runTaskTimer(this, 0L, 20L);//������,���ࡢ�ӳ١�����������һ��,����5���Ǿ���5*20L
	}
	@Override
	public void onDisable()
	{
		if(player!="none233")
		{
			msg+="<��ʾ>�������ѹرա�";
		}
		else
		{
			player="ok";
			msg="<��ʾ>�������ѹرա�";
		}
		getLogger().info("QQGroupMessagePlugin is stoped successfully!");
	}
	
	@EventHandler
	public void onPlayerSay(AsyncPlayerChatEvent event)
	{
		if(player!="none233")
		{
			msg+="]][[<"+event.getPlayer().getName()+">"+event.getMessage();
			//player="ok";
		}
		else
		{
			player="ok";
			msg="<"+event.getPlayer().getName()+">"+event.getMessage();
		}
		
		if(event.getMessage().indexOf("ǩ��")!=-1)
		{
			if(player!="none233")
			{
				msg+="]][[<qd>"+event.getPlayer().getName();
			}
			else
			{
				player="ok";
				msg="<qd>"+event.getPlayer().getName();
			}
		}
		//Bukkit.broadcastMessage("player:"+event.getPlayer().getName()+",msg:"+event.getMessage());
	}
	
	@EventHandler
	public void onPlayerJoin(PlayerJoinEvent event)
	{
		if(player!="none233")
		{
			msg+="]][[<��Ϣ>"+event.getPlayer().getName()+"������";
		}
		else
		{
			player="ok";
			msg="<��Ϣ>"+event.getPlayer().getName()+"������";
		}
	}
	
	@EventHandler
	public void onPlayerQuit(PlayerQuitEvent event)
	{
		if(player!="none233")
		{
			msg+="]][[<��Ϣ>"+event.getPlayer().getName()+"������";
		}
		else
		{
			player="ok";
			msg="<��Ϣ>"+event.getPlayer().getName()+"������";
		}
	}
	
	@EventHandler
	public void onPlayerDeath(PlayerDeathEvent e)
	{
	    Player d = e.getEntity();
	    Player k = e.getEntity().getKiller();
	    String i =d.getDisplayName() + "��" + k.getDisplayName() + "ɱ���ˡ�";
		if(player!="none233")
		{
			msg+="]][[<��Ϣ>" + i;
		}
		else
		{
			player="ok";
			msg="<��Ϣ>" + i;
		}
	}
	
	
    private boolean setupEconomy() {
		if(Bukkit.getPluginManager().isPluginEnabled("Vault")){
			RegisteredServiceProvider<Economy> economyProvider = getServer().getServicesManager()
					.getRegistration(net.milkbowl.vault.economy.Economy.class);
			if (economyProvider != null) {
				economy = economyProvider.getProvider();
			}
			return (economy != null);
		}else{
			return false;
		}
	}
	/*
	public void socket(String player, String msg)
	{
        try 
        {
            //1.�����ͻ���Socket��ָ����������ַ�Ͷ˿�
            Socket socket=new Socket("localhost", 2333);
            //2.��ȡ���������������˷�����Ϣ
            OutputStream os=socket.getOutputStream();//�ֽ������
            PrintWriter pw=new PrintWriter(os);//���������װΪ��ӡ��
            pw.write("<"+player+">"+msg);
            pw.flush();
            socket.shutdownOutput();//�ر������
            //3.��ȡ������������ȡ�������˵���Ӧ��Ϣ
            //InputStream is=socket.getInputStream();
            //BufferedReader br=new BufferedReader(new InputStreamReader(is));
            //String info=null;
            //while((info=br.readLine())!="msg ok!"){
            	//Bukkit.broadcastMessage(br.readLine());
            //}
            //4.�ر���Դ
            //br.close();
            //is.close();
            pw.close();
            os.close();
            socket.close();
        } catch (UnknownHostException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
	}*/
}

