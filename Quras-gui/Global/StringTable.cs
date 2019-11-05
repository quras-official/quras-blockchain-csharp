using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quras_gui.Global
{
    static class StringTable
    {
        public static string[,] DATA = new string [,]
        {
             {
                "WELLCOME TO QURAS COIN WALLET", //0
                "REVOLUTIONARY SOLUTION FOR IOT AND BIG DATA", //1
                "New Wallet", //2
                "Restore", //3
                "WOULD YOU LIKE TO MANUALY SET UP YOUR WALLET?", //4
                "Please Input the wallet path.", //5
                "Quras wallet path", //6
                "Please Input the password.", //7
                "Password", //8
                "Confirm Password", //9
                "Anonymous", //10
                "Next", //11
                "Wallet path is not inputed!", //12
                "Wallet path is not correct!", //13
                "Password is not inputed!", //14
                "Confirm Password is not correct!", //15
                "Quras Welcome", //16
                "New Wallet", //17
                "ENTER YOUR BACKUP PHRASE", //18
                "Continue", //19
                "Open Wallet", //20
                "Password is not correct!", //21
                "Please Input the wallet path.", //22
                "Please Input the password.", //23
                "Newest Version :", //24
                "Update", //25
                "Update logs", //26
                "Update", //27
                "Close", //28
                "Quras Wallet", //29
                "Send", // 30
                "Receive", //31
                "Copy Address", //32
                "Height", //33
                "Connected", //34
                "Transaction History", //35
                "There is no history", //36
                "Verify key was loaded.", //37
                "Loading PK...", //38
                "Loading PK Successed!", //39
                "Loading PK Failed!", //40
                "Loading verify key was failed!", //41
                "Alert", //42
                "Your address has been copied.", //43
                "You can send the coin to other address.", //44
                "You can make a qr code of your address.", //45
                "You can copy your address.", //46
                "This button shows you a qr code of your address.", //47
                "This is your QR Code of address.", //48
                "Send Coin", //49
                "From Address", //50
                "Reciepient Address", //51
                "Please only enter an QRS address. Funds will be lost otherwise.", //52
                "Amount && Assets", //53
                "Please check the assets type.", //54
                "Max", //55
                "Please check the amount.", //56
                "Send", //57
                "You didn't input the from wallet path!\r\nPlease input the from wallet path.", //58
                "The from wallet format is not correct!", //59
                "You didn't input the wallet path!", //60
                "From addr is same as to addr!", //61
                "The wallet format is not correct!", //62
                "You didn't select the assets type!", //63
                "You didn't input the amount!", //64
                "Please input the correct amount!", //65
                "Send amount must be bigger than zero!", //66
                "You didn't select correct Asset!", //67
                "The amount is unsufficient!", //68
                "The Snarks param is not loaded!. Please wait till loaded the params.", //69
                "Receive", //70
                "Address", //71
                "Please input the address that you want to make a qr code.", //72
                "Assets Type", //73
                "Select the asset type.", //74
                "Balance", //75
                " Make QR-Code", //76
                "This wallet support anonymous transaction.", //77
             },
             {
                "QURAS COIN WALLETへようこそ", //0
                "IOTと大容量データのための革命的な解決策", //1
                "新しいウォレット", //2
                "リストア", //3
                "新しいウォレットを作成しますか？", //4
                "ウォレットのパスをご入力してください。", //5
                "Qurasウォレットパス", //6
                "パスワードをご入力してください。", //7
                "パスワード", //8
                "確認パスワード", //9
                "匿名", //10
                "次", //11
                "ウォレットのパスが入力されていません。", //12
                "ウォレットのパスが正しくないです。", //13
                "パスワードが入力されていません。", //14
                "確認パスワードが正しくないです。", //15
                "Quras ようこそ", //16
                "新しいウォレット", //17
                "ウォレットをオープンして、パスワードをご入力してください。", //18
                "持続", //19
                "ウォレットをオープン", //20
                "パスワードが正しくないです。", //21
                "ウォレットのパスをご入力してください。", //22
                "パスワードをご入力してください。", //23
                "新しいヴァージョン :", //24
                "更新", //25
                "ログ更新", //26
                "更新", //27
                "閉じる", //28
                "Quras 財布", //29
                "送金", // 30
                "受金", //31
                "コピーアドレス", //32
                "height", //33
                "Connected", //34
                "トランザクション履歴", //35
                "履歴がありません。", //36
                "認証キー読込が成功いたしました。", //37
                "PK読込中...", //38
                "PK読込が成功いたしました。", //39
                "PK読込が失敗いたしました。", //40
                "認証キー読込が失敗いたしました。", //41
                "アラート", //42
                "あなたのアドレスがコピーされました。", //43
                "他のアドレスへ送金ができます。", //44
                "アドレスのQRコードを生成することができます。", //45
                "アドレスをコピーすることができます。", //46
                "このボタンをクリックすると、QRコードを見れます。", //47
                "アドレスのQRコードです。", //48
                "コイン送金", //49
                "送金元アドレス", //50
                "送金先アドレス", //51
                "QRSアドレスをご入力してください。 入力しないと資金は失われます。", //52
                "金額　&& 資産", //53
                "資産タイプをご確認ください。", //54
                "最大値", //55
                "金額をご確認ください。", //56
                "送金", //57
                "送金元ウォレットパスを入力しなかったです！\r\n送金元ウォレットパスをご入力してください。", //58
                "送金元ウォレット形式が正しくないです。", //59
                "ウォレットパスを入力しなかったです！", //60
                "送金元アドレスと送金先アドレスが同じです。", //61
                "ウォレット形式が正しくないです。", //62
                "資産タイプを選択しなかったです。", //63
                "金額を入力しなかったです。", //64
                "金額を正確にご入力してください。", //65
                "送金額は0以上の金額となる必要があります。", //66
                "正しい資産を選択しなかったです。", //67
                "残高が不足です。", //68
                "Snarksパラメータが読込されませんでした。パラメータが読込されるまでお待ちください。", //69
                "受金", //70
                "アドレス", //71
                "QRコードを作成するアドレスをご入力してください。", //72
                "資産タイプ", //73
                "資産タイプ選択", //74
                "残高", //75
                " QRコード作成", //76
                "ウォレットは匿名のトランザクションをサポートします。", //77
             }
        };
    }
}
