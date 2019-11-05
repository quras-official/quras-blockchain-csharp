using System;
using System.Collections.Generic;

namespace Quras_gui_wpf.Utils
{
    public enum LANG
    {
        EN,
        JP
    }

    public class StringTable
    {
        private static StringTable instance;
        private Dictionary<string, string> dic_en;
        private Dictionary<string, string> dic_jp;

        public static StringTable GetInstance()
        {
            if (instance == null)
            {
                instance = new StringTable();
            }

            return instance;
        }

        public StringTable()
        {
            InitInstance();
        }

        public string GetString(string key, LANG lang = LANG.EN)
        {
            string ret = "";

            try
            {
                switch (lang)
                {
                    case LANG.EN:
                        ret = dic_en[key];
                        break;
                    case LANG.JP:
                        ret = dic_jp[key];
                        break;
                    default:
                        ret = dic_en[key];
                        break;
                }
            }
            catch (Exception)
            {
                switch (lang)
                {
                    case LANG.EN:
                        ret = "Undefined";
                        break;
                    case LANG.JP:
                        ret = "正義されない";
                        break;
                    default:
                        ret = "Undefined";
                        break;
                }
            }
            return ret;
        }

        private void InitInstance()
        {
            InitializeDicEN();
            InitializeDicJP();
        }

        private void InitializeDicEN()
        {
            if (dic_en == null)
                dic_en = new Dictionary<string, string>();

            dic_en.Add("STR_SPLASH_VERSION", "Version");
            dic_en.Add("STR_SPLASH_CHECKING_VERSION_STATUS", "Checking newest version from server.");
            dic_en.Add("STR_SPLASH_CHECKED_VERSION", "The newest version is '{0}'.");

            // Welcome Page
            dic_en.Add("STR_WP_COMMENT_HEADER", "WELCOME TO QURAS WALLET");
            dic_en.Add("STR_WP_COMMENT_BODY", "PRIVACY PROTECTING SMART CONTRACT PLATFORM" + System.Environment.NewLine + "WITH TRANSACTION FEE SHARING FEATURE");
            dic_en.Add("STR_WP_NEW_WALLET", "New Wallet");
            dic_en.Add("STR_WP_RESTORE", "Restore");

            // NewWallet Page
            dic_en.Add("STR_NW_COMMENT_HEADER", "WOULD YOU LIKE TO MANUALY SET UP YOUR WALLET?");
            dic_en.Add("STR_NW_WALLET_PATH", "Please select your wallet path");
            dic_en.Add("STR_NW_ANONYMOUS", "Anonymous");
            dic_en.Add("STR_NW_TRANSPARENT", "Transparent");
            dic_en.Add("STR_NW_STEALTH", "Stealth");
            dic_en.Add("STR_NW_NEXT", "Next");

            dic_en.Add("STR_NW_ERR_WALLET_PATH", "Input the wallet path to save.");
            dic_en.Add("STR_NW_ERR_PATH", "Input the correct path.");
            dic_en.Add("STR_NW_ERR_PASSWORD", "Input the password.");
            dic_en.Add("STR_NW_ERR_CONFIRM_PASSWORD", "Input the correct confirm password.");
            dic_en.Add("STR_NW_SUC_WALLET", "Creating wallet.");
            dic_en.Add("STR_NW_ERR_UNKNOWN", "Unknown Error!");

            // RestoreWallet Page
            dic_en.Add("STR_RW_COMMENT_HEADER", "ENTER YOUR BACKUP PHRASE");
            dic_en.Add("STR_RW_TAG_WALLET_PATH", "Please select your wallet path");
            dic_en.Add("STR_RW_NEXT", "Next");

            dic_en.Add("STR_RW_ERR_WALLET_PATH", "Input the wallet path to open.");
            dic_en.Add("STR_RW_ERR_PASSWORD_INPUT", "Input the password.");
            dic_en.Add("STR_RW_SUCCESS", "Opening the wallet.");
            dic_en.Add("STR_RW_ERR_INCORRECT_PASSWORD", "Password is not correct.");
            dic_en.Add("STR_RW_ERR_UNKNOWN", "Unknown Error!");

            // History Page
            dic_en.Add("STR_HP_TITLE", "TRANSACTION HISTORY");

            // MainWalletWindow Page
            dic_en.Add("STR_MW_HEIGHT", "Height");
            dic_en.Add("STR_MW_CONNECTED", "Connected");
            dic_en.Add("STR_MW_ADDRESS_COPIED_SUCCESS", "Address was copied.");

            // HistoryItem
            dic_en.Add("STR_HI_PENDING", "Pending");
            dic_en.Add("STR_HI_COMPLETED", "Completed");
            dic_en.Add("STR_HI_FROM", "From");
            dic_en.Add("STR_HI_TO", "To");
            dic_en.Add("STR_HI_UNKNOWN", "Unknown");
            dic_en.Add("STR_HI_ANONYMOUS", "Anonymous");
            dic_en.Add("STR_HI_RINGCT", "RingCT");

            // Receiving Page
            dic_en.Add("STR_RP_AMOUNT", "Amount");
            dic_en.Add("STR_RP_GENERATE", "Generate");

            dic_en.Add("STR_RP_ERR_AMOUNT_FORMAT", "Ammount format is fail.");
            dic_en.Add("STR_RP_ERR_INPUT_AMOUNT", "Input the amount to receive.");
            dic_en.Add("STR_RP_ERR_INPUT_FEE_IN_LIMIT", "Input the fee amount within the fee range.");

            // Send Page
            dic_en.Add("STR_SP_SEND", "Send");
            dic_en.Add("STR_SP_RECEIVE_ADDRESS", "Receiving address");
            dic_en.Add("STR_SP_AMOUNT", "Amount");
            
            dic_en.Add("STR_SP_ERR_SELF_TRANSFER", "Self transfer is not avaliable.");
            dic_en.Add("STR_SP_ERR_INCORRECT_AMOUNT", "The balance is not sufficient.");
            dic_en.Add("STR_SP_ERR_INPUT_AMOUNT", "Input the amount field.");
            dic_en.Add("STR_SP_ERR_INCORRECT_RECEIVE_ADDRESS", "Input the receive address field");
            dic_en.Add("STR_SP_ERR_AMOUNT_FORMAT", "Ammount format is not correct.");
            dic_en.Add("STR_SP_SENDING_TX", "Sending transaction...");
            dic_en.Add("STR_SP_SENDING_SUCCESSFULLY", "Sending transaction was successed.");
            dic_en.Add("STR_SP_SEDDING_FAILED", "Please wait till finishing the pending tx.");

            dic_en.Add("STR_SP_ERR_NOT_LOADED_ZK_SNARKS_KEY", "You didn't load ZK-SNARKS Keys.");
            dic_en.Add("STR_ERR_ANONYMOUSE_STEALTH", "Transfering between anonymouse and stealth wallets is not available.");

            // Setting Page
            dic_en.Add("STR_SETTINGS_TITLE", "SETTINGS");
            dic_en.Add("STR_SETTINGS_TITLE_COMMENT", "You can set your wallet configs on this page.");
            dic_en.Add("STR_SETTINGS_LANGUAGE_TITLE", "Language");
            dic_en.Add("STR_SETTINGS_LANGUAGE_COMMENT", "You can set language as you want." + System.Environment.NewLine + "Please choose language that you want.");

            dic_en.Add("STR_SETTINGS_ZKS_TITLE", "Zk-snarks key");
            dic_en.Add("STR_SETTINGS_ZKS_COMMENT", "If you want to use anonymous transaction," + System.Environment.NewLine + "you have to download public key and verify key." + System.Environment.NewLine + "And after that, you have to load zk-snarks module.");
            dic_en.Add("STR_SETTINGS_ZKS_VK_TITLE", "Verifying key");
            dic_en.Add("STR_SETTINGS_ZKS_VK_COMMENT", "Verify key is an zk-snarks sub key. you have to download it.");
            dic_en.Add("STR_SETTINGS_ZKS_PK_TITLE", "Public key");
            dic_en.Add("STR_SETTINGS_ZKS_PK_COMMENT", "Public key is an zk-snarks sub key. you have to download it.");
            dic_en.Add("STR_SETTINGS_ZKS_ZKMODULE_TITLE", "Zk-snarks module");
            dic_en.Add("STR_SETTINGS_ZKS_ZKMODULE_COMMENT", "After downloading keys, you have to load this module.");

            dic_en.Add("STR_SETTINGS_ZKS_VK_DOWNLOADED", "Downloaded");
            dic_en.Add("STR_SETTINGS_ZKS_VK_LOADED", "Loaded");
            dic_en.Add("STR_SETTINGS_ZKS_VK_NOT_DOWNLOADED", "Not Downloaded");
            dic_en.Add("STR_SETTINGS_ZKS_VK_DOWNLOADING", "Downloading");

            dic_en.Add("STR_SETTINGS_ZKS_PK_DOWNLOADED", "Downloaded");
            dic_en.Add("STR_SETTINGS_ZKS_PK_LOADED", "Loaded");
            dic_en.Add("STR_SETTINGS_ZKS_PK_NOT_DOWNLOADED", "Not Downloaded");
            dic_en.Add("STR_SETTINGS_ZKS_PK_DOWNLOADING", "Downloading");

            dic_en.Add("STR_SETTINGS_ZKS_ZKM_DOWNLOADED", "Downloaded");
            dic_en.Add("STR_SETTINGS_ZKS_ZKM_LOADED", "Loaded");
            dic_en.Add("STR_SETTINGS_ZKS_ZKM_NOT_LOADED", "Not Loaded");

            dic_en.Add("STR_SETTINGS_KEYS_LOADING", "Loading");

            dic_en.Add("STR_UPDATE_COMMENT", "Wallet version : {0}" + System.Environment.NewLine + "The newest version: {1}");

            dic_en.Add("STR_SETTING_UPDATE_DOWNLOADING", "Downloading");

            dic_en.Add("STR_SETTINGS_REPAIR_TITLE", "Repair Wallet");
            dic_en.Add("STR_SETTINGS_REPAIR_COMMENT", "You can repair your wallet if your wallet has some problem.");
            dic_en.Add("STR_SETTINGS_BUTTON_DOWNLOAD", "Download");
            dic_en.Add("STR_SETTINGS_BUTTON_LOAD", "Load");
            dic_en.Add("STR_SETTINGS_BUTTON_UPDATE", "Update");
            dic_en.Add("STR_SETTINGS_BUTTON_REPAIR", "Repair");

            dic_en.Add("STR_CLAIM_AVAILABLE", "Available : {0} XQG");
            dic_en.Add("STR_CLAIM_UNAVAILABLE", "Unavailable : {0} XQG");

            dic_en.Add("STR_SETTINGS_UPDATE_TITLE", "Update");
            dic_en.Add("STR_SETTINGS_STATUS_UPDATED", "Updated");
            dic_en.Add("STR_SETTINGS_STATUS_OLDVERSION", "Old version");
            dic_en.Add("STR_SETTINGS_STATUS_FAILED", "Failed");

            dic_en.Add("STR_SETTINGS_CLAIM_TITLE", "Claim XQG");
            dic_en.Add("STR_SETTINGS_BUTTON_CLAIM", "Claim");

            dic_en.Add("STR_SMARTCONTRACT_INVOKE_BUTTON", "Invoke smart contract");
            dic_en.Add("STR_SMARTCONTRACT_DEPLOY_BUTTON", "Deploy smart contract");
            dic_en.Add("STR_SMARTCONTRACT_ADD_ASSET_BUTTON", "Add asset & Issue asset");
            dic_en.Add("STR_SMARTCONTRACT_DEPLOY_COMMENT", "You can deploy the smart contract on this page.");
            dic_en.Add("STR_SMARTCONTRACT_INVOKE_COMMENT", "You can invoke the smart contract on this page." + System.Environment.NewLine +"You can test your smart contract also.");

            dic_en.Add("STR_DSCP_SMART_CONTRACT_INFO", "Smart contract info");
            dic_en.Add("STR_DSCP_SMART_CONTRACT_NAME", "Smart Contract Name");
            dic_en.Add("STR_DSCP_SMART_CONTRACT_VERSION", "Version");
            dic_en.Add("STR_DSCP_SMART_CONTRACT_AUTHOR", "Author");
            dic_en.Add("STR_DSCP_SMART_CONTRACT_EMAIL", "Email");
            dic_en.Add("STR_DSCP_SMART_CONTRACT_DESCRIPTION", "Description");
            dic_en.Add("STR_DSCP_PARAMETERS_TITLE", "Parameters and return types");
            dic_en.Add("STR_DSCP_SMART_CONTRACT_PARAMETERS", "Parameters");
            dic_en.Add("STR_DSCP_SMART_CONTRACT_RETURN_TYPE", "Return types");
            dic_en.Add("STR_DSCP_SMART_CONTRACT_CODE", "Smart contract byte codes");
            dic_en.Add("STR_DSCP_SMART_CONTRACT_CODES", "Codes");
            dic_en.Add("STR_DSCP_BUTTON_LOAD", "Load");
            dic_en.Add("STR_DSCP_INVOCATION_TX_SCRIPT_HASH", "Smart Contract Transaction Script Hash");
            dic_en.Add("STR_DSCP_SMART_CONTRACT_SCRIPT_HASH_COMMENT", "Smart contract script hash");
            dic_en.Add("STR_DSCP_SMART_CONTRACT_SCRIPT_HASH", "Script Hash");
            dic_en.Add("STR_DSCP_SMART_CONTRACT_RESULT_COMMENT", "Smart Contract Test Result");
            dic_en.Add("STR_DSCP_SMART_CONTRACT_RESULT", "Result");
            dic_en.Add("STR_SMART_CONTRACT_FEE", "Fees : {0} XQG");
            dic_en.Add("STR_DSCP_NEED_STORAGE", "Need Storage");
            dic_en.Add("STR_DSCP_NEED_VM", "Need VM");
            dic_en.Add("STR_DSCP_BUTTON_TEST", "Test");
            dic_en.Add("STR_DSCP_BUTTON_DEPLOY", "Deploy");

            dic_en.Add("STR_ISCP_BUTTON_SEARCH", "Search");
            dic_en.Add("STR_ISCP_BUTTON_ADD", "Add");
            dic_en.Add("STR_ISCP_SCRIPT", "Script");
            dic_en.Add("STR_ISCP_RESULTS", "Results");
            dic_en.Add("STR_ISCP_CONTRACT_RESULT", "Smart Contract VM Result");
            dic_en.Add("STR_ISCP_BUTTON_RUN", "Run");

            dic_en.Add("STR_SCP_VALUE", "Value");
            dic_en.Add("STR_ERR_INVOKE_TEST", "Invocation smart contract test was failed.");

            dic_en.Add("STR_BUTTON_YES", "YES");
            dic_en.Add("STR_BUTTON_NO", "NO");

            dic_en.Add("STR_ALERT_DLG_TITLE", "ALERT DIALOG");
            dic_en.Add("STR_ALERT_UPDATE_BODY", "New Quras update is available, would you like to install?");
            dic_en.Add("STR_ALERT_REPAIR_WALLER", "Would you like to repair your wallet?");
            dic_en.Add("STR_ALERT_SEND_TX", "Are you sure to send coin(s)?");

            dic_en.Add("STR_ERR_VK_DOWNLOAD", "VK downloading was failed.");
            dic_en.Add("STR_SUCCESS_VK_DOWNLOAD", "VK downloading was finished successfully.");
            dic_en.Add("STR_ERR_PK_DOWNLOAD", "PK downloading was failed.");
            dic_en.Add("STR_SUCCESS_PK_DOWNLOAD", "PK downloading was finished successfully.");

            dic_en.Add("STR_SUCCESS_LOADING_VERIFYKEY", "VK key loading was finished successfully.");
            dic_en.Add("STR_BEGIN_LOADING_PUBLICKEY", "Loading PK key was started.");
            dic_en.Add("STR_SUCCESS_LOADED_PUBLICKEY", "PK key loading was finished successfully.");
            dic_en.Add("STR_ERR_LOADING_PUBLICKEY", "PK key loading was failed.");
            dic_en.Add("STR_ERR_LOADING_VERIFYKEY", "VK key loading was failed.");
            dic_en.Add("STR_ERR_UPDATE_DOWNLOADING", "Update diff downloading was failed.");
            dic_en.Add("STR_SUC_UPDATE_DOWNLOADING", "Update diff downloading was finished successfully.");
            dic_en.Add("STR_SUC_REPAIR_WALLET", "Repairing wallet was started successfully.");
            dic_en.Add("STR_SUCCESS_LOADED_VERIFYKEY", "VK key was loaded successfuly");

            // Transaction Type
            dic_en.Add("STR_TX_TYPE_CLAIM", "Claim Transaction");
            dic_en.Add("STR_TX_TYPE_INVOCATION", "Invocation Transaction");
            dic_en.Add("STR_TX_TYPE_ISSUE", "Issue Transaction");
            dic_en.Add("STR_TX_TYPE_MINER", "Miner Transaction");

            dic_en.Add("STR_ERR_SM_SCRIPT_HASH_FORMAT", "Please input correct script hash.");

            dic_en.Add("STR_SUC_TX_SUCCESSED", "The transaction was sent successfully.");
            dic_en.Add("STR_ERR_INCOMPLETEDSIGNATUREMESSAGE", "Failed to make transaction.");
            dic_en.Add("STR_ERR_TX_UNSYNCHRONIZEDBLOCK", "The blockchain was not synchronized.");
            dic_en.Add("STR_ERR_TX_INSUFFICIENTFUND", "Insufficient funds.");

            // ADD ASSET PAGE
            dic_en.Add("STR_ERR_ADD_ASSET_IN_CYPT", "Can't add or issue assets with anonymouse or stealth wallet.");
            dic_en.Add("STR_AAP_COMMENT", "You can launch your own asset on blockchain.");
            dic_en.Add("STR_AAP_TITLE", "Add asset");
            dic_en.Add("STR_AAP_REG_ASSET_TITLE", "Register Asset");
            dic_en.Add("STR_AAP_REG_COMMENT1", "You can register your asset on blockchain" + System.Environment.NewLine + "If you want to launch your own asset, you have to register this before.");
            dic_en.Add("STR_AAP_REG_COMMENT2", "* To register asset, you must pay 4,990 XQG token. *");
            dic_en.Add("STR_AAP_FH_ASSET_TYPE", "Asset Type");
            dic_en.Add("STR_AAP_FC_ASSET_TYPE", "Please choose your token type as you want to create.");
            dic_en.Add("STR_AAP_FH_ASSET_NAME", "Asset Name");
            dic_en.Add("STR_AAP_FC_ASSET_NAME", "Please input your asset name that you want to create.");
            dic_en.Add("STR_AAP_FH_ASSET_AMOUNT", "Asset Amount");
            dic_en.Add("STR_AAP_FC_ASSET_AMOUNT", "Please input an total amount of asset to supply." + System.Environment.NewLine + "If you don't want to set limit supply then set the check button.");
            dic_en.Add("STR_AAP_FH_ASSET_PRECISION", "Asset Precision");
            dic_en.Add("STR_AAP_FC_ASSET_PRECISION", "Please input a precision of asset.");
            dic_en.Add("STR_AAP_FH_ASSET_AFEE", "Asset fee for anonymous users");
            dic_en.Add("STR_AAP_FC_ASSET_AFEE", "Please input a fee that will be paied by anonymous account users.");
            dic_en.Add("STR_AAP_FH_ASSET_TFEE_MIN", "Asset fee min value for transparent users");
            dic_en.Add("STR_AAP_FC_ASSET_TFEE_MIN", "Please input a fee min value that will be paied by transparent account users.");
            dic_en.Add("STR_AAP_FH_ASSET_TFEE_MAX", "Asset fee max value for transparent users");
            dic_en.Add("STR_AAP_FC_ASSET_TFEE_MAX", "Please input a fee max value that will be paied by transparent account users.");
            dic_en.Add("STR_AAP_FH_ASSET_FEE_ADDRESS", "Asset Fee Address");
            dic_en.Add("STR_AAP_FC_ASSET_FEE_ADDRESS", "Please input a transparent address that will be received asset fee");
            dic_en.Add("STR_AAP_FH_ASSET_OWNER", "Asset Owner");
            dic_en.Add("STR_AAP_FC_ASSET_OWNER", "Please input your public key, the token owner.");
            dic_en.Add("STR_AAP_FH_ASSET_ADMIN", "Asset Admin");
            dic_en.Add("STR_AAP_FC_ASSET_ADMIN", "Please input your address, the admin of token.");
            dic_en.Add("STR_AAP_FH_ASSET_ISSUER", "Asset Issuer");
            dic_en.Add("STR_AAP_FC_ASSET_ISSUER", "Please input address, the isser of token.");
            dic_en.Add("STR_AAP_BTN_MAKE_TRANSACTION", "Make Transaction");
            dic_en.Add("STR_AAP_HDR_MAKE_TX", "Make transaction & Calculate the XQG Token for fee");
            dic_en.Add("STR_AAP_SCRIPT_HASH_COMMENT", "Transaction Script hash");
            dic_en.Add("STR_AAP_TX_RESULT_COMMENT", "Transaction Result");
            dic_en.Add("STR_AAP_BTN_LAUNCH", "Launch");
            dic_en.Add("STR_AAP_NO_LIMIT", "No limit");
            dic_en.Add("STR_AAP_ASSET_ID", "Asset ID");

            dic_en.Add("STR_ERR_AAP_FIELD", "Please input fields painted red as correct!");
            dic_en.Add("STR_ERR_DUPLICATED_TOKEN", "This named token is already exist in blockchain.");
            dic_en.Add("STR_SUC_TEST_TRANSACTION", "Successfully finished building transaction.");
            dic_en.Add("STR_ERR_TEST_TRANSACTION", "Failed on building transaction.");
            dic_en.Add("STR_INV_NOT_FOUND_CONTRACT", "Not found smart contract");

            dic_en.Add("STR_ERR_NOT_FOUND_ASSET", "Can not find asset id!");
            dic_en.Add("STR_FOUND_ASSET", "Found asset that was registered.");

            dic_en.Add("STR_ERR_ADDR_COPY", "Cannot copy your address.");

            dic_en.Add("STR_ISSUE_TOTALS", "Totals : {0} {1}");

            dic_en.Add("STR_ERR_NOT_INPUT_ADDRESS", "Please input address to send.");
            dic_en.Add("STR_ERR_NOT_INPUT_AMOUNT", "Please input amount to send.");
            dic_en.Add("STR_ERR_ADDRESS_FORMAT", "Address format is not correct.");
            dic_en.Add("STR_ERR_AMOUNT_FORMAT", "Amount format is not correct.");
            dic_en.Add("STR_ERR_NO_OUTPUTS", "Please add outputs.");
            dic_en.Add("STR_ERR_ISSUE_AMOUNT_NOT_SUFFICIENT", "Total amount is bigger than available amount.");

            dic_en.Add("STR_AAP_ISSUE_ASSET_TITLE", "Issue asset");
            dic_en.Add("STR_AAP_ISSUE_ASSET_COMMENT", "You can issue asset that registered on blockchain before." + System.Environment.NewLine + "After issue asset, then you can use this asset to send.");
            dic_en.Add("STR_AAP_ISSUE_ASSET_COMMENT1", "* To issue asset, you must pay 1 XQG token. *");

            dic_en.Add("STR_AAP_ISSUE_ASSET_ID", "Asset ID");
            dic_en.Add("STR_AAP_ISSUE_ASSET_ID_COMMENT", "Please input your asset id that you registered before.");

            dic_en.Add("STR_AAP_ISSUE_ASSET_NAME", "Asset Name");
            dic_en.Add("STR_AAP_ISSUE_ASSET_NAME_COMMENT", "You can get your asset name that you registered.");

            dic_en.Add("STR_AAP_ISSUE_ASSET_ISSUED", "Asset Issued");
            dic_en.Add("STR_AAP_ISSUE_ASSET_ISSUED_COMMENT", "You can get your asset issued amount that you registered.");

            dic_en.Add("STR_AAP_ISSUE_ASSET_PRECISION", "Asset Precision");
            dic_en.Add("STR_AAP_ISSUE_ASSET_PRECISION_COMMENT", "You can get your asset precision that you registered.");

            dic_en.Add("STR_AAP_ISSUE_ASSET_ADMIN", "Asset Admin");
            dic_en.Add("STR_AAP_ISSUE_ASSET_ADMIN_COMMENT", "You can get your asset admin that you registered.");

            dic_en.Add("STR_AAP_ISSUE_ASSET_OWNER", "Asset Owner");
            dic_en.Add("STR_AAP_ISSUE_ASSET_OWNER_COMMENT", "You can get your asset owner that you registered.");

            dic_en.Add("STR_AAP_ISSUE_BTN_MAKE_OUTPUT", "Make Output");
            dic_en.Add("STR_AAP_ISSUE_MAKE_TX_HEADER", "Make transaction & Calculate the XQG Token for fee");

            dic_en.Add("STR_AAP_ISSUE_TX_RESULT", "Transaction Hash");

            dic_en.Add("STR_TOPD_TITLE", "Transaction Output Window");
            dic_en.Add("STR_TOPD_ASSET_TYPE", "Asset Type");
            dic_en.Add("STR_TOPD_AVAILABLE_ISSUE", "Available Amount");
            dic_en.Add("STR_TOPD_AVAILABLE_AMOUNT", "Available Amount");
            dic_en.Add("STR_TOPD_ADDRESS", "Address");
            dic_en.Add("STR_TOPD_AMOUNT", "Amount");

            dic_en.Add("STR_MENU_ASSET_PRECISION", "Precision : {0}");
            dic_en.Add("STR_MENU_ASSET_TOTAL_SUPPLY", "Total Supply : {0}");

            dic_en.Add("STR_TASK_MESSAGE_PK_KEY_LOAD_START", "PK Key loading" + System.Environment.NewLine + "PK Key was started.");
            dic_en.Add("STR_TASK_MESSAGE_PK_KEY_LOAD_FINISHED", "PK Key loading" + System.Environment.NewLine + "PK Key was loaded successfully.");
            dic_en.Add("STR_TASK_MESSAGE_PK_KEY_LOAD_FAILED", "PK Key loading" + System.Environment.NewLine + "PK Key was failed.");

            dic_en.Add("STR_TASK_MESSAGE_SEND_TX_START", "Sending transaction");
            dic_en.Add("STR_TASK_MESSAGE_SEND_TX_FINISHED", "Finished sending transaction");
            dic_en.Add("STR_TASK_MESSAGE_SEND_TX_FAILED", "Failed sending transaction");

            dic_en.Add("STR_TASK_MESSAGE_SEND_TX_MESSAGE", "{0}" + System.Environment.NewLine + "From : {1}" + System.Environment.NewLine + "To : {2}" + System.Environment.NewLine + "Amount : {3}{4}");

            dic_en.Add("STR_SP_SPENDABLE", "Spendable : {0} {1}");
            dic_en.Add("STR_SP_FEE", "Fee : {0} {1}  ~  {2} {3}");
            dic_en.Add("STR_SP_AFEE", "Fee : {0} {1}");

            dic_en.Add("STR_WALLET_REPAIR", "CM Tree Error, repair your wallet!");
            dic_en.Add("STR_SEND_COIN_ERROR_WAITING", "Please wait till finishing the previous tx.");
            dic_en.Add("STR_WALLET_DUPPLICATION_ERROR", "Wallet is already running.");

            dic_en.Add("STR_SP_ERR_TRANSPARENT_TOKEN", "Address types must be transparent type.");
            dic_en.Add("STR_SP_ERR_ANONYMOUSE_TOKEN", "Address types must be anonymouse type.");

            dic_en.Add("STR_ADD_ASSET_DEPLOY", "Would you like to deploy token?");
            dic_en.Add("STR_ISSUE_ASSET_QUESTION", "Would you like to issue tokens?");
            dic_en.Add("STR_FEE_FREE", "Free");
        }

        private void InitializeDicJP()
        {
            if (dic_jp == null)
                dic_jp = new Dictionary<string, string>();

            dic_jp.Add("STR_SPLASH_VERSION", "バージョン");
            dic_jp.Add("STR_SPLASH_CHECKING_VERSION_STATUS", "Checking newest version from server.");
            dic_jp.Add("STR_SPLASH_CHECKED_VERSION", "The newest version is '{0}'.");

            // Welcome Page
            dic_jp.Add("STR_WP_COMMENT_HEADER", "QURAS COIN WALLETへようこそ");
            dic_jp.Add("STR_WP_COMMENT_BODY", "取引手数料共有機能付きスマートコントラクトプラットフォーム");
            dic_jp.Add("STR_WP_NEW_WALLET", "新しいウォレット");
            dic_jp.Add("STR_WP_RESTORE", "リストア");

            // NewWallet Page
            dic_jp.Add("STR_NW_COMMENT_HEADER", "新しいウォレットを作成しますか？");
            dic_jp.Add("STR_NW_WALLET_PATH", "ウォレットのパスをご入力してください。");
            dic_jp.Add("STR_NW_ANONYMOUS", "匿名");
            dic_jp.Add("STR_NW_TRANSPARENT", "透明タイプ");
            dic_jp.Add("STR_NW_STEALTH", "ステルスタイプ");
            dic_jp.Add("STR_NW_NEXT", "次");


            dic_jp.Add("STR_NW_ERR_WALLET_PATH", "ウォレットのパスをご入力してください。");
            dic_jp.Add("STR_NW_ERR_PATH", "ウォレットのパスが正しくありません。");
            dic_jp.Add("STR_NW_ERR_PASSWORD", "パスワードをご入力してください。");
            dic_jp.Add("STR_NW_ERR_CONFIRM_PASSWORD", "確認パスワードが正しくないです。");
            dic_jp.Add("STR_NW_SUC_WALLET", "ウォレット生成中");
            dic_jp.Add("STR_NW_ERR_UNKNOWN", "アンノウンエラー");

            // RestoreWallet Page
            dic_jp.Add("STR_RW_COMMENT_HEADER", "ウォレットをオープンして、パスワードをご入力してください。");
            dic_jp.Add("STR_RW_TAG_WALLET_PATH", "ウォレットのパスをご入力してください。");
            dic_jp.Add("STR_RW_NEXT", "次");

            dic_jp.Add("STR_RW_ERR_WALLET_PATH", "ウォレットのパスをご入力してください。");
            dic_jp.Add("STR_RW_ERR_PASSWORD_INPUT", "パスワードをご入力してください。");
            dic_jp.Add("STR_RW_SUCCESS", "ウォレットを開けている");
            dic_jp.Add("STR_RW_ERR_INCORRECT_PASSWORD", "パスワードが正しくないです。");
            dic_jp.Add("STR_RW_ERR_UNKNOWN", "アンノウンエラー");

            // History Page
            dic_jp.Add("STR_HP_TITLE", "トランザクションの履歴");

            // MainWalletWindow Page
            dic_jp.Add("STR_MW_HEIGHT", "ブロックの状態");
            dic_jp.Add("STR_MW_CONNECTED", "接続することができ");
            dic_jp.Add("STR_MW_ADDRESS_COPIED_SUCCESS", "アドレスがコピーされました。");

            // HistoryItem
            dic_jp.Add("STR_HI_PENDING", "未確認");
            dic_jp.Add("STR_HI_COMPLETED", "完成");
            dic_jp.Add("STR_HI_FROM", "入金");
            dic_jp.Add("STR_HI_TO", "出金");
            dic_jp.Add("STR_HI_UNKNOWN", "道の");
            dic_jp.Add("STR_HI_ANONYMOUS", "匿名");
            dic_jp.Add("STR_HI_RINGCT", "RingCT");

            // Receiving Page
            dic_jp.Add("STR_RP_AMOUNT", "金額");
            dic_jp.Add("STR_RP_GENERATE", "生成");

            dic_jp.Add("STR_RP_ERR_AMOUNT_FORMAT", "金額を正確にご入力してください。");
            dic_jp.Add("STR_RP_ERR_INPUT_AMOUNT", "金額を入力しなかったです。");

            // Send Page
            dic_jp.Add("STR_SP_SEND", "送金");
            dic_jp.Add("STR_SP_RECEIVE_ADDRESS", "送金先アドレス");
            dic_jp.Add("STR_SP_AMOUNT", "金額");
            dic_jp.Add("STR_RP_ERR_INPUT_FEE_IN_LIMIT", "手数料を範囲内で入力してください。");

            dic_jp.Add("STR_SP_ERR_SELF_TRANSFER", "セルフ転送ができない。");
            dic_jp.Add("STR_SP_ERR_INCORRECT_AMOUNT", "残高が不足です。");
            dic_jp.Add("STR_SP_ERR_INPUT_AMOUNT", "金額を入力しなかったです。");
            dic_jp.Add("STR_SP_ERR_INCORRECT_RECEIVE_ADDRESS", "送金先をご入力してください。");
            dic_jp.Add("STR_SP_ERR_AMOUNT_FORMAT", "金額を正確にご入力してください。");
            dic_jp.Add("STR_SP_SENDING_TX", "送金処理しています。");
            dic_jp.Add("STR_SP_SENDING_SUCCESSFULLY", "送金が成功いたしました。");
            dic_jp.Add("STR_SP_SEDDING_FAILED", "保留中のTXを終えるまで待ってください。");

            dic_jp.Add("STR_SP_ERR_NOT_LOADED_ZK_SNARKS_KEY", "ZK-SNARKSキーをロードしていません。");
            dic_jp.Add("STR_ERR_ANONYMOUSE_STEALTH", "匿名とステルスの間での譲渡はできません。");

            // Setting Page
            dic_jp.Add("STR_SETTINGS_TITLE", "設定");
            dic_jp.Add("STR_SETTINGS_TITLE_COMMENT", "このページでウォレットの設定をすることができます。");
            dic_jp.Add("STR_SETTINGS_LANGUAGE_TITLE", "言語");
            dic_jp.Add("STR_SETTINGS_LANGUAGE_COMMENT", "ここで言語を設定することができます。" + System.Environment.NewLine + "変更するための言語をご選択してください。");

            dic_jp.Add("STR_SETTINGS_ZKS_TITLE", "Zk-snarksキー");
            dic_jp.Add("STR_SETTINGS_ZKS_COMMENT", "Anonymous transactionを利用するためにはベリファイキーとパブリックキーをダウンロードしてください。");
            dic_jp.Add("STR_SETTINGS_ZKS_VK_TITLE", "認証キー");
            dic_jp.Add("STR_SETTINGS_ZKS_VK_COMMENT", "ベリファイキーはzk-snarksのキーです。" + System.Environment.NewLine + "「download」ボタンをクリックして、ダウンロードしてください。");
            dic_jp.Add("STR_SETTINGS_ZKS_PK_TITLE", "公開鍵");
            dic_jp.Add("STR_SETTINGS_ZKS_PK_COMMENT", "パブリックキーはzk-snarksのキーです。" + System.Environment.NewLine + "「download」ボタンをクリックして、ダウンロードしてください。");
            dic_jp.Add("STR_SETTINGS_ZKS_ZKMODULE_TITLE", "Zk-snarksモジュール");
            dic_jp.Add("STR_SETTINGS_ZKS_ZKMODULE_COMMENT", "ベリファイキーとパブリックキーをダウンロードした後に" + System.Environment.NewLine + "zk - snarksモジュールをロードすることができます。");

            dic_jp.Add("STR_SETTINGS_ZKS_VK_DOWNLOADED", "ダウンロード済み");
            dic_jp.Add("STR_SETTINGS_ZKS_VK_LOADED", "ロードされた");
            dic_jp.Add("STR_SETTINGS_ZKS_VK_NOT_DOWNLOADED", "降り積載失敗");
            dic_jp.Add("STR_SETTINGS_ZKS_VK_DOWNLOADING", "降り積載中");

            dic_jp.Add("STR_SETTINGS_ZKS_PK_DOWNLOADED", "ダウンロード済み");
            dic_jp.Add("STR_SETTINGS_ZKS_PK_LOADED", "ロードされた");
            dic_jp.Add("STR_SETTINGS_ZKS_PK_NOT_DOWNLOADED", "降り積載失敗");
            dic_jp.Add("STR_SETTINGS_ZKS_PK_DOWNLOADING", "降り積載中");

            dic_jp.Add("STR_SETTINGS_ZKS_ZKM_DOWNLOADED", "ダウンロード済み");
            dic_jp.Add("STR_SETTINGS_ZKS_ZKM_LOADED", "ロードされた");
            dic_jp.Add("STR_SETTINGS_ZKS_ZKM_NOT_LOADED", "積載失敗");

            dic_jp.Add("STR_SETTINGS_KEYS_LOADING", "積載中");

            dic_jp.Add("STR_UPDATE_COMMENT", "ウォレットバージョン : {0}" + System.Environment.NewLine + "最新バージョン: {1}");

            dic_jp.Add("STR_SETTINGS_REPAIR_TITLE", "ウォレットを修理する");
            dic_jp.Add("STR_SETTINGS_REPAIR_COMMENT", "ウォレットに問題がある場合は「修復」ボタンをクリックして、問題を解決することができます。");
            dic_jp.Add("STR_SETTINGS_BUTTON_DOWNLOAD", "ダウンロード");
            dic_jp.Add("STR_SETTINGS_BUTTON_LOAD", "負荷");
            dic_jp.Add("STR_SETTINGS_BUTTON_UPDATE", "更新");
            dic_jp.Add("STR_SETTINGS_BUTTON_REPAIR", "修復");
            dic_jp.Add("STR_CLAIM_AVAILABLE", "利用可能 : {0} XQG");
            dic_jp.Add("STR_CLAIM_UNAVAILABLE", "利用できません : {0} XQG");

            dic_jp.Add("STR_SETTINGS_UPDATE_TITLE", "更新");
            dic_jp.Add("STR_SETTINGS_STATUS_UPDATED", "更新しました");
            dic_jp.Add("STR_SETTINGS_STATUS_OLDVERSION", "旧バージョン");
            dic_jp.Add("STR_SETTINGS_STATUS_FAILED", "失敗した");

            dic_jp.Add("STR_SETTINGS_CLAIM_TITLE", "請求 XQG");
            dic_jp.Add("STR_SETTINGS_BUTTON_CLAIM", "請求");

            dic_jp.Add("STR_SMARTCONTRACT_INVOKE_BUTTON", "スマート契約を呼び出す");
            dic_jp.Add("STR_SMARTCONTRACT_DEPLOY_BUTTON", "スマート契約を展開する");
            dic_jp.Add("STR_SMARTCONTRACT_ADD_ASSET_BUTTON", "資産を追加して資産を発行");
            dic_jp.Add("STR_SMARTCONTRACT_DEPLOY_COMMENT", "このページでスマートコントラクトをデプロイできます。");
            dic_jp.Add("STR_SMARTCONTRACT_INVOKE_COMMENT", "このページでスマートコントラクトを呼び出すことができます。" + System.Environment.NewLine + "あなたもあなたの賢い契約をテストすることができます。");

            dic_jp.Add("STR_DSCP_SMART_CONTRACT_INFO", "スマートコントラクト情報");
            dic_jp.Add("STR_DSCP_SMART_CONTRACT_NAME", "スマートコントラクト名");
            dic_jp.Add("STR_DSCP_SMART_CONTRACT_VERSION", "ヴァージョン");
            dic_jp.Add("STR_DSCP_SMART_CONTRACT_AUTHOR", "著者");
            dic_jp.Add("STR_DSCP_SMART_CONTRACT_EMAIL", "メール");
            dic_jp.Add("STR_DSCP_SMART_CONTRACT_DESCRIPTION", "説明");
            dic_jp.Add("STR_DSCP_PARAMETERS_TITLE", "パラメータと戻り値の型");
            dic_jp.Add("STR_DSCP_SMART_CONTRACT_PARAMETERS", "パラメータ");
            dic_jp.Add("STR_DSCP_SMART_CONTRACT_RETURN_TYPE", "戻り値の型");
            dic_jp.Add("STR_DSCP_SMART_CONTRACT_CODE", "スマートコントラクトバイトコード");
            dic_jp.Add("STR_DSCP_SMART_CONTRACT_CODES", "コード");
            dic_jp.Add("STR_DSCP_BUTTON_LOAD", "ロード");
            dic_jp.Add("STR_DSCP_INVOCATION_TX_SCRIPT_HASH", "スマートコントラクトトランザクションスクリプトハッシュ");
            dic_jp.Add("STR_DSCP_SMART_CONTRACT_SCRIPT_HASH_COMMENT", "スマートコントラクトスクリプトハッシュ");
            dic_jp.Add("STR_DSCP_SMART_CONTRACT_SCRIPT_HASH", "スクリプトハッシュ");
            dic_jp.Add("STR_DSCP_SMART_CONTRACT_RESULT_COMMENT", "スマートコントラクトテスト結果");
            dic_jp.Add("STR_DSCP_SMART_CONTRACT_RESULT", "結果");
            dic_jp.Add("STR_SMART_CONTRACT_FEE", "手数料：{0}XQG");
            dic_jp.Add("STR_DSCP_NEED_STORAGE", "必要なストーレジ");
            dic_jp.Add("STR_DSCP_NEED_VM", "必要なVM");
            dic_jp.Add("STR_DSCP_BUTTON_TEST", "テスト");
            dic_jp.Add("STR_DSCP_BUTTON_DEPLOY", "ブロックチェインに実装");

            dic_jp.Add("STR_ISCP_BUTTON_SEARCH", "検索");
            dic_jp.Add("STR_ISCP_BUTTON_ADD", "追加");
            dic_jp.Add("STR_ISCP_SCRIPT", "スクリプト");
            dic_jp.Add("STR_ISCP_RESULTS", "結果");
            dic_jp.Add("STR_ISCP_CONTRACT_RESULT", "スマートコントラクトVM結果");
            dic_jp.Add("STR_ISCP_BUTTON_RUN", "実行");

            dic_jp.Add("STR_SCP_VALUE", "値");
            dic_jp.Add("STR_ERR_INVOKE_TEST", "スマートコントラクトテストが失敗しました。");

            dic_jp.Add("STR_BUTTON_YES", "はい");
            dic_jp.Add("STR_BUTTON_NO", "いいえ");

            dic_jp.Add("STR_ALERT_DLG_TITLE", "確認ダイアログ");
            dic_jp.Add("STR_ALERT_UPDATE_BODY", "更新できるQurasの新版本がありますが、更新しますか?");
            dic_jp.Add("STR_ALERT_REPAIR_WALLER", "ウォレットファイルを修正しますか？");
            dic_jp.Add("STR_ALERT_SEND_TX", "必ずコインを送りますか?");

            dic_jp.Add("STR_ERR_VK_DOWNLOAD", "VKダウンロードが失敗しました。");
            dic_jp.Add("STR_SUCCESS_VK_DOWNLOAD", "VKダウンロードが成功しました。");
            dic_jp.Add("STR_ERR_PK_DOWNLOAD", "PKダウンロードが失敗しました。");
            dic_jp.Add("STR_SUCCESS_PK_DOWNLOAD", "PKダウンロードが成功しました。");

            dic_jp.Add("STR_SUCCESS_LOADING_VERIFYKEY", "VKキーの読み込みが正常に終了しました。");
            dic_jp.Add("STR_BEGIN_LOADING_PUBLICKEY", "PKキーの読み込みが開始されました。");
            dic_jp.Add("STR_SUCCESS_LOADED_PUBLICKEY", "PKキーのロードが正常に終了しました。");
            dic_jp.Add("STR_ERR_LOADING_PUBLICKEY", "PKキーの読み込みに失敗しました。");
            dic_jp.Add("STR_ERR_LOADING_VERIFYKEY", "VKキーの読み込みに失敗しました。");
            dic_jp.Add("STR_ERR_UPDATE_DOWNLOADING", "更新差をダウンロードできませんでした。");
            dic_jp.Add("STR_SUC_UPDATE_DOWNLOADING", "更新差分のダウンロードが正常に終了しました。");
            dic_jp.Add("STR_SUC_REPAIR_WALLET", "財布を修理することに成功した。");
            dic_jp.Add("STR_SUCCESS_LOADED_VERIFYKEY", "VKキーの読み込みが正常に終了しました。");

            // Transaction Type
            dic_jp.Add("STR_TX_TYPE_CLAIM", "請求取引");
            dic_jp.Add("STR_TX_TYPE_INVOCATION", "呼び出しトランザクション");
            dic_jp.Add("STR_TX_TYPE_ISSUE", "トークン発行トランザクション");
            dic_jp.Add("STR_TX_TYPE_MINER", "マイナートランザクション");

            dic_jp.Add("STR_ERR_SM_SCRIPT_HASH_FORMAT", "正しいスクリプトハッシュを入力してください。.");

            dic_jp.Add("STR_SUC_TX_SUCCESSED", "トランザクションは正常に送信されました。");
            dic_jp.Add("STR_ERR_INCOMPLETEDSIGNATUREMESSAGE", "取引に失敗しました。");
            dic_jp.Add("STR_ERR_TX_UNSYNCHRONIZEDBLOCK", "ブロックチェーンは同期されませんでした。");
            dic_jp.Add("STR_ERR_TX_INSUFFICIENTFUND", "残高不足。");

            // ADD ASSET PAGE
            dic_jp.Add("STR_ERR_ADD_ASSET_IN_CYPT", "匿名またはステルスウォレットで資産を追加または発行することはできません。");
            dic_jp.Add("STR_AAP_COMMENT", "あなたはブロックチェーンであなた自身の資産を起動することができます。");
            dic_jp.Add("STR_AAP_TITLE", "資産を追加");
            dic_jp.Add("STR_AAP_REG_ASSET_TITLE", "資産を登録する");
            dic_jp.Add("STR_AAP_REG_COMMENT1", "あなたはあなたの資産をブロックチェーンに登録することができます" + System.Environment.NewLine + "あなた自身の資産を起動したい場合は、事前にこれを登録する必要があります。");
            dic_jp.Add("STR_AAP_REG_COMMENT2", "* 資産を登録するには、4,990 XQGトークンを支払う必要があります。 *");
            dic_jp.Add("STR_AAP_FH_ASSET_TYPE", "資産タイプ");
            dic_jp.Add("STR_AAP_FC_ASSET_TYPE", "作成したいトークンの種類を選択してください。");
            dic_jp.Add("STR_AAP_FH_ASSET_NAME", "資産名");
            dic_jp.Add("STR_AAP_FC_ASSET_NAME", "作成したいアセット名を入力してください。");
            dic_jp.Add("STR_AAP_FH_ASSET_AMOUNT", "資産額");
            dic_jp.Add("STR_AAP_FC_ASSET_AMOUNT", "供給する資産の総額を入力してください。" + System.Environment.NewLine + "供給制限を設定したくない場合は、チェックボタンを設定します。");
            dic_jp.Add("STR_AAP_FH_ASSET_PRECISION", "資産の精度");
            dic_jp.Add("STR_AAP_FC_ASSET_PRECISION", "資産の精度を入力してください。");
            dic_jp.Add("STR_AAP_FH_ASSET_AFEE", "匿名ユーザーのアセット料金");
            dic_jp.Add("STR_AAP_FC_ASSET_AFEE", "匿名アカウントの利用者が支払う料金を入力してください。");
            dic_jp.Add("STR_AAP_FH_ASSET_TFEE_MIN", "透過的なユーザーの資産使用料最小値");
            dic_jp.Add("STR_AAP_FH_ASSET_TFEE_MAX", "透過的なユーザーの資産使用料の最大値");
            dic_jp.Add("STR_AAP_FC_ASSET_TFEE_MIN", "透過的なアカウントの利用者が入力する料金最小値を入力してください。");
            dic_jp.Add("STR_AAP_FC_ASSET_TFEE_MAX", "透過的なアカウントの利用者が入力する料金最大値を入力してください");
            dic_jp.Add("STR_AAP_FH_ASSET_FEE_ADDRESS", "資産手数料アドレス");
            dic_jp.Add("STR_AAP_FC_ASSET_FEE_ADDRESS", "資産使用料を受け取る透明アドレスを入力してください");
            dic_jp.Add("STR_AAP_FH_ASSET_OWNER", "資産所有者");
            dic_jp.Add("STR_AAP_FC_ASSET_OWNER", "あなたの公開鍵、トークンの所有者を入力してください。");
            dic_jp.Add("STR_AAP_FH_ASSET_ADMIN", "資産管理者");
            dic_jp.Add("STR_AAP_FC_ASSET_ADMIN", "住所、トークンの管理者を入力してください。");
            dic_jp.Add("STR_AAP_FH_ASSET_ISSUER", "アセット発行者");
            dic_jp.Add("STR_AAP_FC_ASSET_ISSUER", "住所、トークンの発行者を入力してください。");
            dic_jp.Add("STR_AAP_BTN_MAKE_TRANSACTION", "トランザクションを構築");
            dic_jp.Add("STR_AAP_HDR_MAKE_TX", "トランザクションを構築 & 手数料としてXQGトークンを計算する");
            dic_jp.Add("STR_AAP_SCRIPT_HASH_COMMENT", "トランザクションスクリプトハッシュ");
            dic_jp.Add("STR_AAP_TX_RESULT_COMMENT", "取引結果");
            dic_jp.Add("STR_AAP_BTN_LAUNCH", "起動する");
            dic_jp.Add("STR_AAP_NO_LIMIT", "制限なし");
            dic_jp.Add("STR_AAP_ASSET_ID", "資産ID");

            dic_jp.Add("STR_ERR_AAP_FIELD", "赤く塗られている欄を正しく入力してください。");
            dic_jp.Add("STR_ERR_DUPLICATED_TOKEN", "この名前付きトークンは,すでにブロックチェーンに存在します。");
            dic_jp.Add("STR_SUC_TEST_TRANSACTION", "構築トランザクションを正常に終了しました。");
            dic_jp.Add("STR_ERR_TEST_TRANSACTION", "トランザクションの構築に失敗しました。");
            dic_jp.Add("STR_INV_NOT_FOUND_CONTRACT", "スマート契約が見つかりません");

            dic_jp.Add("STR_ERR_NOT_FOUND_ASSET", "アセットIDが見つかりません。");
            dic_jp.Add("STR_FOUND_ASSET", "登録されている資産が見つかりました。");
            dic_jp.Add("STR_ERR_ADDR_COPY", "住所をコピーできません。");
            dic_jp.Add("STR_ISSUE_TOTALS", "合計 : {0} {1}");

            dic_jp.Add("STR_ERR_NOT_INPUT_ADDRESS", "送付先住所を入力してください。");
            dic_jp.Add("STR_ERR_NOT_INPUT_AMOUNT", "送る金額を入力してください。");
            dic_jp.Add("STR_ERR_ADDRESS_FORMAT", "アドレス形式が正しくありません。");
            dic_jp.Add("STR_ERR_AMOUNT_FORMAT", "金額の形式が正しくありません。");
            dic_jp.Add("STR_ERR_NO_OUTPUTS", "出力を追加してください。");
            dic_jp.Add("STR_ERR_ISSUE_AMOUNT_NOT_SUFFICIENT", "合計金額が利用可能金額よりも大きい。");

            dic_jp.Add("STR_AAP_ISSUE_ASSET_TITLE", "発行資産");
            dic_jp.Add("STR_AAP_ISSUE_ASSET_COMMENT", "あなたは以前にブロックチェーンに登録された資産を発行することができます。" + System.Environment.NewLine + "資産の発行後、この資産を使用して送信できます。");
            dic_jp.Add("STR_AAP_ISSUE_ASSET_COMMENT1", "*資産を発行するには、1 XQGトークンを支払わなければなりません。*");

            dic_jp.Add("STR_AAP_ISSUE_ASSET_ID", "資産ID");
            dic_jp.Add("STR_AAP_ISSUE_ASSET_ID_COMMENT", "以前に登録したアセットIDを入力してください。");

            dic_jp.Add("STR_AAP_ISSUE_ASSET_NAME", "資産名");
            dic_jp.Add("STR_AAP_ISSUE_ASSET_NAME_COMMENT", "あなたはあなたが登録したあなたの資産名を得ることができます。");

            dic_jp.Add("STR_AAP_ISSUE_ASSET_ISSUED", "発行された資産");
            dic_jp.Add("STR_AAP_ISSUE_ASSET_ISSUED_COMMENT", "あなたが登録したあなたの資産発行額を得ることができます。");

            dic_jp.Add("STR_AAP_ISSUE_ASSET_PRECISION", "アセットプレシジョン");
            dic_jp.Add("STR_AAP_ISSUE_ASSET_PRECISION_COMMENT", "あなたはあなたが登録したあなたの資産の正確さを得ることができます。");

            dic_jp.Add("STR_AAP_ISSUE_ASSET_ADMIN", "資産管理者");
            dic_jp.Add("STR_AAP_ISSUE_ASSET_ADMIN_COMMENT", "あなたはあなたが登録したあなたの資産管理者を得ることができます。");

            dic_jp.Add("STR_AAP_ISSUE_ASSET_OWNER", "資産所有者");
            dic_jp.Add("STR_AAP_ISSUE_ASSET_OWNER_COMMENT", "あなたはあなたが登録したあなたの資産所有者を得ることができます。");

            dic_jp.Add("STR_AAP_ISSUE_BTN_MAKE_OUTPUT", "出力する");
            dic_jp.Add("STR_AAP_ISSUE_MAKE_TX_HEADER", "取引を行い、手数料のためにXQGトークンを計算する");

            dic_jp.Add("STR_AAP_ISSUE_TX_RESULT", "トランザクションハッシュ");

            dic_jp.Add("STR_TOPD_TITLE", "トランザクション出力ウィンドウ");
            dic_jp.Add("STR_TOPD_ASSET_TYPE", "資産タイプ");
            dic_jp.Add("STR_TOPD_AVAILABLE_ISSUE", "利用可能額");
            dic_jp.Add("STR_TOPD_AVAILABLE_AMOUNT", "利用可能額");
            dic_jp.Add("STR_TOPD_ADDRESS", "住所");
            dic_jp.Add("STR_TOPD_AMOUNT", "量");

            dic_jp.Add("STR_MENU_ASSET_PRECISION", "精度 : {0}");
            dic_jp.Add("STR_MENU_ASSET_TOTAL_SUPPLY", "総供給 : {0}");

            dic_jp.Add("STR_TASK_MESSAGE_PK_KEY_LOAD_START", "PKキーローディング" + System.Environment.NewLine + "PK鍵のロードが開始されました。");
            dic_jp.Add("STR_TASK_MESSAGE_PK_KEY_LOAD_FINISHED", "PKキーローディング" + System.Environment.NewLine + "PKキーは正常にロードされました。");
            dic_jp.Add("STR_TASK_MESSAGE_PK_KEY_LOAD_FAILED", "PKキーローディング" + System.Environment.NewLine + "PKキーが失敗しました。");

            dic_jp.Add("STR_TASK_MESSAGE_SEND_TX_START", "送信トランザクション");
            dic_jp.Add("STR_TASK_MESSAGE_SEND_TX_FINISHED", "送信済みトランザクション");
            dic_jp.Add("STR_TASK_MESSAGE_SEND_TX_FAILED", "トランザクションの送信に失敗しました");

            dic_jp.Add("STR_TASK_MESSAGE_SEND_TX_MESSAGE", "{0}" + System.Environment.NewLine + "から : {1}" + System.Environment.NewLine + "に : {2}" + System.Environment.NewLine + "量 : {3}{4}");

            dic_jp.Add("STR_SP_SPENDABLE", "柔軟な : {0} {1}");
            dic_jp.Add("STR_SP_FEE", "料金 : {0} {1}  ~  {2} {3}");
            dic_jp.Add("STR_SP_AFEE", "料金 : {0} {1}");
            dic_jp.Add("STR_WALLET_REPAIR", "CMツリーのエラー、あなたの財布を修復！");
            dic_jp.Add("STR_SEND_COIN_ERROR_WAITING", "TX前の仕上げまで待ってください。");
            dic_jp.Add("STR_WALLET_DUPPLICATION_ERROR", "財布はもう走っています。");
            dic_jp.Add("STR_SP_ERR_TRANSPARENT_TOKEN", "アドレス型は透過型でなければなりません。");
            dic_jp.Add("STR_SP_ERR_ANONYMOUSE_TOKEN", "アドレス タイプは,匿名タイプである必要があります。");
            dic_jp.Add("STR_ADD_ASSET_DEPLOY", "トークンを展開しますか?");
            dic_jp.Add("STR_ISSUE_ASSET_QUESTION", "トークンを発行しますか。");
            dic_jp.Add("STR_FEE_FREE", "無料");
        }
    }
}
