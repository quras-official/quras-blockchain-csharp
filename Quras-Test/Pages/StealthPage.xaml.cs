using Pure;
using Pure.IO;
using Pure.IO.Json;
using Pure.Core;
using Pure.Core.RingCT.Types;
using Pure.Cryptography;
using Pure.Wallets;
using Pure.Wallets.StealthKey;
using Pure.Core.RingCT.Impls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;

namespace Quras_Test.Pages
{
    /// <summary>
    /// Interaction logic for StealthPage.xaml
    /// </summary>
    public partial class StealthPage : UserControl
    {
        StealthKeyPair keys;
        List<KeyPair> RingSigKeys = new List<KeyPair>();
        BigInteger keyImage;
        byte[] message = { 0x45, 0x32, 0x54, 0x32 };
        
        public StealthPage()
        {
            InitializeComponent();
        }

        private void btnRandom_Click(object sender, RoutedEventArgs e)
        {
            byte[] payloadKey = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(payloadKey);
            }
            txbPayloadPrivKey.Text = payloadKey.ToHexString();

            byte[] viewKey = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(viewKey);
            }
            txbViewPrivKey.Text = viewKey.ToHexString();
        }
        
        private void txbPayloadPrivKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txbPayloadPrivKey.Text.Length == 64 && txbViewPrivKey.Text.Length== 64)
            {
                keys = new StealthKeyPair(txbPayloadPrivKey.Text.HexToBytes(), txbViewPrivKey.Text.HexToBytes());
                txbPayloadPubKey.Text = keys.PayloadPubKey.ToString();
                txbViewPubKey.Text = keys.ViewPubKey.ToString();
                txbAddress.Text = Wallet.ToStealthAddress(keys);
            }
        }

        private void txbViewPrivKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txbPayloadPrivKey.Text.Length == 64 && txbViewPrivKey.Text.Length == 64)
            {
                keys = new StealthKeyPair(txbPayloadPrivKey.Text.HexToBytes(), txbViewPrivKey.Text.HexToBytes());
                txbPayloadPubKey.Text = keys.PayloadPubKey.ToString();
                txbViewPubKey.Text = keys.ViewPubKey.ToString();
                txbAddress.Text = Wallet.ToStealthAddress(keys);
            }
        }

        private void btnOneTimeKey_Click(object sender, RoutedEventArgs e)
        {
            if (txbAddress.Text.Length > 0)
            {
                StealthKeyPair oneTimeKey = Wallet.ToStealthKeyPair(txbAddress.Text);

                byte[] oneTimePrivKey = new byte[32];
                using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(oneTimePrivKey);
                }

                KeyPair oneTimeRandomKey = new KeyPair(oneTimePrivKey);

                txbOneTimeKey.Text = oneTimeKey.GenPaymentOneTimeAddress(oneTimePrivKey);

                keys.CheckPaymentPubKeyHash(oneTimeRandomKey.PublicKey, "");
            }
        }

        public BigInteger GetKeyImage(byte[] privKey, Pure.Cryptography.ECC.ECPoint pubKey)
        {
            BigInteger priv = new BigInteger(privKey.Reverse().Concat(new byte[1]).ToArray());
            byte[] hashP = Crypto.Default.Hash256(pubKey.ToString().HexToBytes());
            BigInteger hashPub = new BigInteger(hashP.Reverse().Concat(new byte[1]).ToArray());

            return priv * hashPub;
        }

        private void btnGen_Click(object sender, RoutedEventArgs e)
        {
            RingSigKeys.Clear();

            for (int i = 0; i < 4; i ++)
            {
                byte[] privKey = new byte[32];
                using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(privKey);
                }

                KeyPair oneTimeRandomKey = new KeyPair(privKey);
                RingSigKeys.Add(oneTimeRandomKey);
            }

            using (RingSigKeys[0].Decrypt())
            {
                txbSignPrivKey.Text = RingSigKeys[0].PrivateKey.ToHexString();
                txbSignPubKey.Text = RingSigKeys[0].PublicKey.ToString();

                keyImage = GetKeyImage(RingSigKeys[0].PrivateKey, RingSigKeys[0].PublicKey);

                txbKeyImage.Text = keyImage.ToByteArray().Reverse().ToArray().ToHexString();
            }
        }

        private void btnGenRingSig_Click(object sender, RoutedEventArgs e)
        {
            using (RingSigKeys[0].Decrypt())
            {
                IEnumerable<Pure.Cryptography.ECC.ECPoint> pubKeys = RingSigKeys.Select(p => p.PublicKey);
                RingSignature sign = message.RingSign(pubKeys.ToList(), RingSigKeys[0].PrivateKey, keyImage.ToByteArray().Reverse().ToArray(), 0);
                sign.RingVerify(message, pubKeys.ToList());
            }
        }

        private void BtnMakeFrom_Click(object sender, RoutedEventArgs e)
        {
            byte[] payloadKey = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(payloadKey);
            }
            txbFromPayloadPriv.Text = payloadKey.ToHexString();

            byte[] viewKey = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(viewKey);
            }
            txbFromViewPriv.Text = viewKey.ToHexString();

            keys = new StealthKeyPair(txbFromPayloadPriv.Text.HexToBytes(), txbFromViewPriv.Text.HexToBytes());

            txbFromPayloadPub.Text = keys.PayloadPubKey.ToString();
            txbFromViewPub.Text = keys.ViewPubKey.ToString();
            txbFromAddress.Text = Wallet.ToStealthAddress(keys);
        }

        private void BtnMakeTo_Click(object sender, RoutedEventArgs e)
        {
            byte[] payloadKey = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(payloadKey);
            }
            txbToPayloadPriv.Text = payloadKey.ToHexString();

            byte[] viewKey = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(viewKey);
            }
            txbToViewPriv.Text = viewKey.ToHexString();

            keys = new StealthKeyPair(txbToPayloadPriv.Text.HexToBytes(), txbToViewPriv.Text.HexToBytes());

            txbToPayloadPub.Text = keys.PayloadPubKey.ToString();
            txbToViewPub.Text = keys.ViewPubKey.ToString();
            txbToAddress.Text = Wallet.ToStealthAddress(keys);
        }

        private void BtnMakeTx_Click(object sender, RoutedEventArgs e)
        {
            StealthKeyPair FromAddr = new StealthKeyPair(txbFromPayloadPriv.Text.HexToBytes(), txbFromViewPriv.Text.HexToBytes());
            StealthKeyPair ToAddr = Wallet.ToStealthKeyPair(txbToTxAddress.Text);
            Fixed8 amount = Fixed8.Parse(txbAmount.Text);

            // =====================================    STEP 1 => Make One-Time Key from ToAddr    =======================================
            byte[] r = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(r);
            }
            r = "1111111111111111111111111111111111111111111111111111111111111111".HexToBytes();
            Pure.Cryptography.ECC.ECPoint R = Pure.Cryptography.ECC.ECCurve.Secp256r1.G * r;
            string OneTimeKey = ToAddr.GenPaymentOneTimeAddress(r);

            { // Calculate the One-Time PrivKey
                StealthKeyPair ToAddrWithPriv = new StealthKeyPair(txbToPayloadPriv.Text.HexToBytes(), txbToViewPriv.Text.HexToBytes());

                byte[] oneTimePrivKey = ToAddrWithPriv.GenOneTimePrivKey(R);
                byte[] oneTimeOrgPubKey = ToAddr.GenPaymentPubKeyHash(r);
                Pure.Cryptography.ECC.ECPoint oneTimePubKey = Pure.Cryptography.ECC.ECCurve.Secp256r1.G * oneTimePrivKey;
                byte[] oneTimePub = oneTimePubKey.EncodePoint(true);
            }
            // =====================================                 end                           =======================================

            // =====================================     STEP 2 => Make TX                         =======================================
            {
                // Make From Address Information
                byte[] from_r = new byte[32];
                using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(from_r);
                }

                Pure.Cryptography.ECC.ECPoint from_R = Pure.Cryptography.ECC.ECCurve.Secp256r1.G * from_r;
                string from_onetimekey = FromAddr.GenPaymentOneTimeAddress(from_r);
                byte[] from_onetimePriv = FromAddr.GenOneTimePrivKey(from_R);
                Fixed8 from_amount = Fixed8.One * 100;
                // Make To Destination Information

                // Make Signature
                List<CTCommitment> inSK = new List<CTCommitment>();
                List<CTKey> inPK = new List<CTKey>();
                List<MixRingCTKey> inPKIndex = new List<MixRingCTKey>();
                List<Pure.Cryptography.ECC.ECPoint> destinations = new List<Pure.Cryptography.ECC.ECPoint>();
                List<Fixed8> amounts = new List<Fixed8>();
                int mixin = 3;

                byte[] out_amount = new byte[32];
                byte[] out_mask = new byte[32];

                for (int i = 0; i < 1; i++)
                {
                    byte[] sk = SchnorrNonLinkable.GenerateRandomScalar(); // One-Time Key private Key
                    Pure.Cryptography.ECC.ECPoint pk = Pure.Cryptography.ECC.ECCurve.Secp256r1.G * sk; // One-Time Key Public Key

                    byte[] mask = SchnorrNonLinkable.GenerateRandomScalar();

                    byte[] b_amount = amount.ToBinaryFormat().ToBinary();

                    Pure.Cryptography.ECC.ECPoint C_i_in = RingCTSignature.GetCommitment(mask, b_amount);

                    out_amount = ScalarFunctions.Add(out_amount, b_amount);
                    out_mask = ScalarFunctions.Add(out_mask, mask);

                    CTKey key = new CTKey(pk, C_i_in);
                    MixRingCTKey mixRingKey = new MixRingCTKey(new UInt256(), 0xff, 0xff);

                    CTCommitment i_insk = new CTCommitment(sk, mask);
                    inSK.Add(i_insk);

                    inPK.Add(key);
                    inPKIndex.Add(mixRingKey);
                }
                
                Pure.Cryptography.ECC.ECPoint dest_pk = Pure.Cryptography.ECC.ECPoint.DecodePoint(ToAddr.GenPaymentPubKeyHash(r), Pure.Cryptography.ECC.ECCurve.Secp256r1);
                destinations.Add(dest_pk);

                amounts.Add(Fixed8.One * 100);

                RingCTSignatureType sig = RingCTSignature.Generate(inSK, inPKIndex, destinations, amounts, Fixed8.Zero, mixin, Pure.Core.Blockchain.GoverningToken.Hash, Fixed8.Zero);
                sig.AssetID = Blockchain.GoverningToken.Hash;
                bool sig_ret = RingCTSignature.Verify(sig, Fixed8.Zero);

                {
                    StealthKeyPair ToAddrWithPriv = new StealthKeyPair(txbToPayloadPriv.Text.HexToBytes(), txbToViewPriv.Text.HexToBytes());

                    try
                    {
                        //byte[] privKey = "db8df9b666a41337e1c4a7a90639a55e8d6d052ccfaca930f7af42642e6e3cde".HexToBytes();
                        //Fixed8 dstAmount = RingCTSignature.DecodeRct(sig, privKey, 0);
                        //Fixed8 dstAmount = RingCTSignature.DecodeRct(sig, ToAddrWithPriv.GenOneTimePrivKey(R), 0);
                    }
                    catch (Exception ex)
                    {
                        string str = ex.ToString();
                    }
                }
                
                RingConfidentialTransaction tx = new RingConfidentialTransaction(sig);

                tx.Attributes = new TransactionAttribute[0];
                tx.Inputs = new CoinReference[0];
                tx.Outputs = new TransactionOutput[0];
                tx.Scripts = new Witness[0];
                tx.RHashKey = R;

                byte[] byTx = tx.ToArray();
                string hexByTx = "820001e17950861e3425837e95cef9b60d14139b4b39108ecdd6f88e46ef99916d54ec012103217bb363cadb4b6069268560dc766af19633bcb11ea36a5a2cdcf2c1137e912a2103ad5f0e13d6acccd0bc567dec782a7b55181fcf3859a86f96722ff45e18c54a03210361fd17d91d27cfecff9955bfbf3970e2d037e882f8a7e651bece87a5e2a189192102d7e59cd69e18e58a7f395c492c633f6dd2729b1d61b59a8195c7aea4091b088621032ff5eb357432fde9ae24ef8899441939a0ad9e729b4eb1ab250a2beacbdc14f32102b7680e2fb3c6e1d8146177994bd2929941fbd18644245ca05d5359087341b0782103e0dae734af3339225a3e1e4f85929eb2f15f0b529fb19249e0cb2409a92a9b9d21035b5f3e6f6e2bcfc67faa74fff0cf8eb44ab228fcfc9f90539c04a9be29070aa62103b1f5f652bc60f9919c8f41ab349a1e165f2b00748d34406e0380e7aea41a16b921039b8c0de8d9bb08fec3bfce9f67b77c20bfffdc7add72deaf5160cd84768c4eb621038f9b4d7378331aa51d1f0593aea64dfca75a3197cc48840e0d450873db104b1c21035357e273aeefda03cb7699774047c2dd753ceb43bfc06198dad23b866f7401252102eec34b3931678d987b6da5646cfda723fd7d3402bcfb2bacf0773a9f411eb79d21034ef11a5a660bf0d1a6ce2bcbdeedbd36320e61643f8bff3b1f5810614650685b21032bb64ec6dbbb0763130cfccb3df5571ae3ac94e3d7c17f2f56874ce1bed3129321020c0d7b21c0be762c7f9c6be20f0efc4b2cde83bde154fcea09dbf066f3b734762102877effa070f419e36c033c2ff184ed622e916228b3eb8c872e2af78ceef3c43f210399d0188bc0ed16fcd8e74983934c372a05329d00d2bfab835a28c961b7eefab52103834bd01574db337cbc2ab0f584be0d585315ed895f6918d1830eec39ac67ecd221020470cb81570e614d67c922e18e01d6a5e30029502878956cd3c22dac14cd5d7c2103249a881db417bdcf41386e8dff560fda26077b9725e3bc7ef225d605d96dce9d210279aeccd59815019805d38fdca5cd2e192278149bcf1e5a5bfb8f93212b01ba7a210281389482ea291470cee6087bb474177aeff81820779a10ed9f8df7d4de0855332102718d6036725c13963125c351903ec76b8a1173757c49616602350ec2671aa6d121031fc31171bd4591c863969c620f7b2bdbd8fc55aafffc4a78686c7fc58dae769f2103218e376d7c555eb29b6502cbff39d02a536ae02a80b968560339d549b6cd4a5f21022437eae1d2f4c7a7065db416d7cbd193e8f69b93d7813d2a594d5222f4663fb72103fd68cb5405c10923784de31a89859c779f1ca72fb9d99e288a95e1eebca5e1e62103576cad991589fdab334892d216917a72233ac163a72f74567e7d553ee96ea6ce210342e07c90a9a9d05973cf4565294ebca6ba567c685cb006efac62d80213dbe94a210304c598fc009686e85fb085abbd6ca38878270caf600d896ec365a90f57b889392102db438de29043b8e4717f486d37e98043cf3571c314685e4fc2c674630404b5722103d4f7f4c5e8d64a3e63d9d9e8b3f27597e93ad93cbbb3da4e6662917b18daef3f21035537e7b3c7caf45bfbea0a78ad1400e6e14444e160b01837062d8a05bd323fe021039200e5040d74e259b5f0dc566ce5c88e12db0786bc17cdd140d2241b4a0d92792102d5d7e19d6a08fb41b2a8cb9cc033aab873fd9234832b052f90ce080de5f486062103b134736c413d30f0df6bdac23a22e7042a33b5676fc84d3ebbc4ae8e6f94294d2102cee8adc988eddf7e64f17a1d61f30058b144dc565da73215e51ad81c7eed131f210383a08cd6e7dcbf5806e94df18431ce2aeca3dcd51b5279e60feec0e4294922dd2103a314fc22c1f68cd83dd4a958143290c3559516ae35ddf8f9c0b093a8f02ec0cb2102d5c2ca5b723952ad62d3228faafe3227520801f36b87dea6e6428ee13d1eeba221024ffb2caf74b67d9c9b84dd8420e85cacec9f28bcce5db7e4ec1c4f5044469f6821022b3e169c2d6b355d68ba9fc818f3658aa3ff9cf4d8e056e1ba8b334025a57c8b21036810b59e5ccd0dcf686a5585f5a6d511ec46b93c6398dff328b2e8d5b2fcc3c72102d635ae00ff439a507e46c41af9d74f233950b43369700816ff93ea9980752aaf2102e0d2867451097502fa5ee7486c7202b40316b71265acdd5cbc64ff76e1fb7cdc2103097fee4c0075cb724f56091a5221808868ec5d95bbe71642bc0562b087db48ad210360956081c5a848125469adf574470de42aedfcfd099510d0e2260c70f5d312452102a11945e2d4413189659d6f6026097176d18ca74af564651fc547b4962d1029fc210301fd1c4e655e633457f67c2fe4c983c6c787511b8364d1e554e4e22aa66a141d21028e01f7db2c47d73e62c0375849bdf797b8bb1df4a12973d1b9046c3a70207eb12103c08db46bd259d2923ef3126b46f9182be130baa56fc56a4f9cbffd92e6fcdf442103a2e4bfbdedad70157929c69f5c7019bf3b29d5d45b97d3894e651b1ada7486782102262e4392a7122f506a510d12ef6da5a51ee6b08d51ecf2e9eae3682c94d6860f2102b8b3bacae0786f8880ff14599c1d39c8d1833d40afe8b3e3314e88c8957c788621024503675159984fb838a959ccac4d2f350dd51ef0af445fdbfda055ff9868b30e2102444da93dce7c423bad3bd549bbaab5f11c710004bb21413ecefd5a92086f185d21025116d5ac15bd40893eb9c2402e185da5dca232fa4f6b316a793de898a96a559f21025531e319cff67b84769f4959a79fcacfdf28bdb3fa51044ffedb34e338f5962521022237e884d38052250004ce050117aa707d77dfc68391558524a1976a52eed212210261590d77afa61b4c6e19f30dd78a6c0d7a45d9dcb21365b3b40bd54e57e2823721035fab8400d2e3754018e1ae25e4f51fc62b915e13e73f1bc92967c96646f625f521029695e025f89224c1f0bd10a47284b8b7bc455f0a646c8f076d6b5c39e21dc12321029ed39cc2268a969babb3de96fedf635d47234f00bc0c29419c8b6e7f697c7c1520714f4623aa9b777b56ef8c99893f0dad257722075881c5bd8069f6b2e982355a20d1a26421035618bc34df237dd901927bc2c0af3bb62ee77e8dafece7f4ae12ca20fdd6d426e3530ea0e9a515d5f1358aa7206f95276cff51b85f3b16795fc54a7020d3d20ab4bd0b3c01670f89f2521866418091b3c07e4e1b56ed4f4ec8ecb464c52069705b8cab9732553f625f6a12423a5c3676d47b7f52b5cbad4d43034a90cda720e8fa4dd0f97fc683b193d2744dac966b0dece38a81cc363e1fc36818552bc77e20498255c4ada8ee5acc4f94955542b4804cb61dcebe2398abf96c88eb03fc4e7d20c164a1a4704c5260845dee57c9df31352fc0c7c81b7a4a6d0b28a97c00ca5734207e0848cff08b9f30398dc2a3c3eb32baa3875f93f664398aa3580678b018827220f959c61fad60954f5146e1603115a22d06f6ffaac3834f6828456de80e4d998620b97f8b0098162aac6d37e9fe1547777c98faaf50919f80772dac219886e42c48208d7570e1b3ff4b056dc59076a2d74072f8a6e1e77d030bc560c70f471a94f22d20b5170be085cfcab0f0035da374fd797844a4d76ba18bc5c0926b2a05857ede25204071d8ac79d680a3ff65f704ade9b46e227a8dedf36681199c01dc3b7d1cdb52203d1b06cb1724b74411fc0cf0c79db4f27d9d2b27a808fa42c0dfbefb5d804ee720ee5f68b9d6daf1ba3f0ba4a2fcd1d1c6c25282577fd6336d29f8122d3821505120cf7fc1a2f12d317ad0da29cbac144b18b8698be556e1878edbcdb2a7fcb7cd0c202b7ea26b253a2375a0a23aed818d647eaacb579eaa550dd4b0db0847fd0c15da20eeff17ce0adb6fd06b3ad654f13f6b2d46b9c4bbd465797a41b0ccb0bcf62284209d3a32c5f6af5a85ee6b1d1cb0d05d0dad237f9c0dc88ed1a9b4f891b87c568320271f4cb1757c058e676dc2e2d27c07ae0775f4614f8cfc8405aabae2a68c53d22081251a629d3b39d9fc26f213253c024b97dacbaae994fd5cfcae5706ccae30d020779e95394c29f6426e62f334901e5c4613fdc0e8f445aa02190343e99b1bb3af2098aafec1c99722c4ede6bb4f3952f80812e13fd8d8997b4ecfe3d340ca254e9f2079ca01f2ef2d80f7265a0d4aee92d6dac03da88a78643b402dde0523b43c6f4b209db4981225b9a91d18d265e61131cf4630010c5c8aee5820667af805420d55bc20c730052e68b774294d5bfb1a69696037eb0b85148d922d1e8ca4e12793d4e03d20f392e3b373fcd95d6109abdf49bb2a0e9fb7aa4dd3d9e96dd5b484fd7d571fcf207a8ce859ebaa266cde9686dc914ccd76c2df315ac86ae489ef931d38e75afc6320f80131bc026ff3b33b9a6787b7b274d3edd93a2ba0e3bc8cc53d6763c8c6d80a2000566f25d5c55f9573016d62923084f70bf8b82ac2c4b5a01e71adf91bd402a320792b85b42807ba40f990072c7d9bd74fac1cd1ca0c365231fe401ff515884d2420beeb1b7365e55e382dcd716f7fecd350af0bb639ad3e6f484fd05a2da960b4b420b45ed002459597803dacd74e04ef04065cf6679ee5c292254c0605c14024deb2207d20267d995df3b66e6ba70c7cc66f95a49d226a9ca300bc70e39560c4e94d8f20d7e87951f77bbfa1d1211ac84409c4f14009f0c3cbe08c48104198e0a2c59c02205732970b441feb61f7b017036c09d4df1fd40a416727a890cc7bc1647354865920d64612f97c1c144b98112806eb2e717d734c24e9d2325e9c4242ec3a1cec8fc92047548e5a0d477588de2d8cc880301c6f70a41aaa4ee84124a95e5ba987ccb5e0205685cb46d6fb8e41faa60533896cdaaae0a3787162377797a3212f423420cb72204b6760fcc453a16f3109fb5d46287c708e4acba257140bc64fce9304b3b407a6200a96adae09503e9e79e471f3f2afcedb00a656d76256ff8693c461543595b5f3201a1ce848bf022f3b602edaa79f71a2aa60caca80bb30d9a37306a8dd4b6aaf87206fd1742ecaf8b495b0fbb58821ccead715ff2544af863df5a7dd4496a5c9aaf220bc91e1cd6b9d71329caf2748276f11badc5394214f7485d8285450e60adfaf44206d23546ca54343623f1b8d2438eb58848f409a95577b3d0c32c6c5b755b7d5cd201aa4af9dd42a1ef3bb41289f882e4698678d5951627ebc2ad94b29c6b1ff051920b7c4f3ea1085aec750ac255cbe7d0fbc963d71c40187df8ea88642b5098a44ff203b2f1830845fc43291a49469956ff43dfa2c939f18516f8b0377350c0821b04b20363f3fadc8133085c61842431c462855dd1873cbff125396f0b8390eed83d9ed20b0556376266f676ab87a6b5ee0b00a2b0f24d38e859baf219a520e2be34944c7207e43ab7f96951483f4e5d96cfbd7fd38ece9cd891c994a493b441d4e689de3ca207229c39f023530940980c642bd4cd3d96d9373bb850d1fd961507ba169d4c073200bae4566e01efc381a06f79439751c586aa6b1e2b0f6b53eff5e80b3f097d2632067eed67a3f6ff5e5edbd3c7a814a7a682276fe596ff16148899c75e46dd5b62a203033034bae421318b27690e58fffda6516c2d96598b6aedd5bcabfb27be8bd6920071b1af4d85c657659109507453f23deb212d5e6d656e6ceb20605bb0baeff7220c719561fee9009ec6472a0635a4ec732228edf85777c5917500ebbb4c1ae9ebb20af494b2de22d94bb7e21c9ad6e1a3e29bfa60efa4a00a95c21cf3dfaba15c67a2046036a52b6e9c3a6133d6597fe0645da979b994866aedf890d074837bd0efd1620f41ace5bfd9887d662a5513fa7922b7c94391fe5a23156b06a1879204a9e0f5520ebfafaa4cafc52a833fb354237d1f545392ebb18fc702ae5e1cef72628c360e920dda8a2166df04f56a9942c1d176d2e9ccbb774d53a7fa90a37ceeea9088b0793209a4d07cdd6ca90b6c3794fca88d58555493bc9ab130a18de7f3b369a77cdbb3220e8f3d06ae6e3105f46df3e5a98fc47c9531767e905e9ea174a53101963c864b901000100010001000100010001000100010001002103ee9d89508b62c03ca87e431d1db03c09c4fb45d1032ddd8ecc066fabaee1ef9b0100010021037bc2525a58093c52f42a5e335dfe3920baa7cf38c14adaf7e16b1f16ae239eb02102b5f774cd9414bebcbb9eee33c57fd5273749da2d05d336c0ef447e674ecde2802102ece5a6022b9545721ae8b6e13123e1cf5b9dc3b6453891daedc3daccbf4db4862103db5f0a885c23ebcb9fc993ff92917a5ad55eebd32e2a0c08c7c525095d3e35b72103181bae40314955fa2f27b71cd9fab7e081dbe8223d91638881175389bbed6a00010021029796ab0109612f16eb602b3fbb2445e8ef285a2090743eeffdcbefdd9ca2667601000100010001000100010021021ae5149bf80ff59c3309c3776bce60da777c136cab559fa0fa5896f7a7b6ce09010021027f8a8071c7939f1e6159e86df722207649bbe323271fc27888271f6a16f192310100210352b793cd577d3b81b4256cf07ef63431faebe4d700e22dfe5d80f23947faf3af0100010021021f086c123a8325bf8aa5ec4c60ab61077eea2165ada9b4ace0dd31418f29224a010001000100010001000100010001000100010001000100010001000100010001000100010001000100010001000100010001000100010001000100210325bf64a726282a5e9379229b46800b6e7cbdda9c7356d47502043d3d815c806f200000000000000000000000000000000000000000000000000000000000000000030000000200000020c8bee05ec6105eefc99cf9af0fd13ca4ffa768bf44eecb9d9896dcc7ca682c94202caf907367317ad20cfb63c829cd4e235377a27964dac92607e6d73fa148bd0020a596e6a611c3baad601596344c87ddbb3a204938eab62f8a99c7846ab2313e5620a6844ee3e77435adcb14d71fdcd3b487fff0aebd440034179f69047c16f48fc020e757dd6abcebbe484cc3a2467ffafbd7e8030f6deb268cca610eeb8715cf185f20c7fe6677f8bbef425e5c59ee1e31739ce057b6ed3da4cec57eefe68f43813ab002000000200ca3efbd379baa6fcae04a8b27f53c0d94c5d63b33378e2b4b789a9c6506573b20931626d807da172aed2379ade960ee9c41e1483818e02997880ff14a925e090c20bd46c9e3a60af628e292f1fa45fd4b30f284c5353a765174cb63fe4f1cc879c1030000000100000002233e40eaa423117365fa49a81badc3943861aab09ba00ac28fbd977b275a042803c9955a2ce0b524a10b5280005fe06186e4869c791c6c903d8ea463b8061b8e9002ecea8dd024916c7bb84e568eeeb2c90e2de3477b7b2be7d85b29ea248784fa4c03ed3088b6dfa4c58d66919c2f9670c8afb378aaec18736a39149cf605c15ee7e3032e6257ec4e3786f34860871fc8aa2d5fd2e223eca9ffd2a4d10aa943b4e2434402dff481b747b5bfbee148352d743f79ca1be6135baed4840cb1833b6de12b218d012000000000000000000000000000000000000000000000000000000000000000002000000000000000000000000000000000000000000000000000000002540be4000381e5712bfb78c5145dc323fa412035f01d8a7c841813ba21fc98975bf98745930102e09346fa1005a8e6b39abbf46f12a288ddbbac33f5814e6502e74b0dda4734490325bf64a726282a5e9379229b46800b6e7cbdda9c7356d47502043d3d815c806f020217e617f0b6443928278f96999e69a23a4f2c152bdf6d6cdf66e5b80282d4ed00000000";

                byte[] tmpbytx = hexByTx.HexToBytes();

                RingConfidentialTransaction tx1 = byTx.AsSerializable<RingConfidentialTransaction>();
                RingConfidentialTransaction tx2 = tmpbytx.AsSerializable<RingConfidentialTransaction>();
                
                // Check the dest address if my address
                {
                    StealthKeyPair ToAddrWithPriv = new StealthKeyPair(txbToPayloadPriv.Text.HexToBytes(), txbToViewPriv.Text.HexToBytes());
                    ToAddrWithPriv.GetPaymentPubKeyFromR(tx1.RHashKey);

                    for (int i = 0; i < tx1.RingCTSig[0].outPK.Count; i++)
                    {
                        if (tx1.RingCTSig[0].outPK[i].dest.ToString() == Pure.Cryptography.ECC.ECPoint.DecodePoint(ToAddrWithPriv.GetPaymentPubKeyFromR(tx1.RHashKey), Pure.Cryptography.ECC.ECCurve.Secp256r1).ToString())
                        {
                            string myaddress = "myaddress";
                        }
                    }
                }
                // end

                bool tx_sig_ret = RingCTSignature.Verify(tx1.RingCTSig[0], Fixed8.Zero);
            }
            // =====================================     STEP 2 => End                             =======================================
        }
    }
}
