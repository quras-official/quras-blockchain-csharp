using Pure.Cryptography;
using Pure.Cryptography.ECC;
using Pure.IO;
using Pure.IO.Caching;
using Pure.SmartContract;
using Pure.VM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pure.Core
{
    public abstract class Blockchain : IDisposable, IScriptTable
    {
        public static event EventHandler<BlockNotifyEventArgs> Notify;
        public static event EventHandler<Block> PersistCompleted;

        public const uint SecondsPerBlock = 18;
        public const uint DecrementInterval = 2000000;
        public static readonly uint[] GenerationAmount = { 64, 56, 48, 40, 32, 24, 16, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8 };

        public static readonly TimeSpan TimePerBlock = TimeSpan.FromSeconds(SecondsPerBlock);

        public static readonly ECPoint[] StandbyValidators = Settings.Default.StandbyValidators.OfType<string>().Select(p => ECPoint.DecodePoint(p.HexToBytes(), ECCurve.Secp256r1)).ToArray();

        public static bool IsTestRingCT = true;
#pragma warning disable CS0612
        public static readonly RegisterTransaction GoverningToken = new RegisterTransaction
        {
            AssetType = AssetType.GoverningToken,
            Name = "[{\"lang\":\"en\",\"name\":\"XQC\"}]",
            Amount = Fixed8.FromDecimal(888888888),
            Precision = 8,
            Owner = ECCurve.Secp256r1.Infinity,
            Admin = (new[] { (byte)OpCode.PUSHT }).ToScriptHash(),
            Attributes = new TransactionAttribute[0],
            Inputs = new CoinReference[0],
            Outputs = new TransactionOutput[0],
            Scripts = new Witness[0],
            A_Fee = Fixed8.Satoshi * 1000000,
            T_Fee = Fixed8.Zero,
            T_Fee_Min = Fixed8.Zero,
            T_Fee_Max = new Fixed8(1) * 100000000
        };

        public static readonly RegisterTransaction UtilityToken = new RegisterTransaction
        {
            AssetType = AssetType.UtilityToken,
            Name = "[{\"lang\":\"en\",\"name\":\"XQG\"}]",
            //Amount = Fixed8.FromDecimal(GenerationAmount.Sum(p => p) * DecrementInterval),
            Amount = Fixed8.FromDecimal(888888888),
            Precision = 8,
            Owner = ECCurve.Secp256r1.Infinity,
            Admin = (new[] { (byte)OpCode.PUSHF }).ToScriptHash(),
            Attributes = new TransactionAttribute[0],
            Inputs = new CoinReference[0],
            Outputs = new TransactionOutput[0],
            Scripts = new Witness[0],
            A_Fee = Fixed8.Satoshi * 1000000,
            T_Fee = Fixed8.Zero,
            T_Fee_Min = Fixed8.Zero,
            T_Fee_Max = new Fixed8(1) * 100000000
        };
#pragma warning restore CS0612

        public static RingConfidentialTransaction GetRingConfidentialTransaction(int coin_id = 0)
        {
            string str_coin;
            string governinghash = GoverningToken.Hash.ToString();
            string utilityhash = UtilityToken.Hash.ToString();

            string governinghash_new = "";
            string utilityhash_new = "";

            for (int i = governinghash.Length - 2; i >= 2; i -=2)
            {
                governinghash_new += governinghash.Substring(i, 2);
            }

            for (int i = utilityhash.Length - 2; i >= 2; i -= 2)
            {
                utilityhash_new += utilityhash.Substring(i, 2);
            }

            if (coin_id == 0)
                str_coin = "820001" +
                           governinghash_new +
                           "012102d21870f5226f19f7a50ca667c11c0a243d120a0a27d9f3a39a793e720872596321025c6d5a338d4c92a1d0df4cd9b7d9ee67b7273cf3dfedc2870055fa74d091aead2102681ad8bc16f48942b565861f376f88ceedae74f1b8f9c1c70a3c295db7670bfa2103ab471c85a9b402d95ed819f38026f71713b3144c2733017d0417afc42ba7a1ca2102ff83eb0a4d459ff2d2e8b9fe3fdd604c5a34926809d16d4ba0ed6e91ffdd0a2d2102ae48af8bd9ef9a712482b20725f6e3a9c8d5e091de2f0ef540914fb5cc9fedfc2103944ec3e30fcb84824b7dc1b1b76609614210d0549c2b67cfe09e0a0eb5f53ce82102bcf0abea435e738e914616971a3d67695bd2e0be6b4277e35f5b6b10947017462103d5b326a216b74a0e37333eeb6f07d63e517fa8d4b38b69dcfffbeba36818a77b210244381dedd468c99805f674a5175eea399682ded0706a265190a40d6033c3f5ca21026daa3353ad1022c10201cd72eb67910a92be468b47a11df0da3ff711ddf33fc62103df2b6152e51a9e3a47c1ff939a338e82c7758a89e73db1d33f1757b3290a5b6e2102600d31243a6aec6deb4231f5419f4f5b26c01c8cb0f21085c56ef19a0d79e8292102d53236198c09f49eb0814e3621fc67cd46672713a0677363f3bb730e986707f921031cd207418d5235165be0b666455e5de41f13381746b7618e2be4ba72576461e721022d09fd0298d15ebb9184e98f463cdc408b2bfba5fbac847b61616804bab4ad1a21023800597b75f156911d1f0b5a3d9878394d53c3ec35e76b1c746270f86828632921032791c1450cba66c75e6010b11019cf03a3873415ddde8f7116d8b59c54e212fd2103af74b6bfb4911fe65cbb52ee5210db6688f737309bb91f5585ee76df86764ed5210333a8912096599ef532ce58b145bcfb08ba8a2c4f1e355af84edce74e50587baf2102eb034a07deecce160adef0c4f2e782ca9fb239284baa0f02edec9107a0b979f42102647cba64012e429eb6211790548194e6bf675e31f75127222097723962c0b9c821037de9266ba267b17f426ddde83baa12348d2425032d88e5a9171523ec3fe906b221023cdf153b9b9bda258b017ba49d6f5e5be4c80e005e5c5821bf2ddb257901201b2102e6e71c9be1457977333ea52316deab959a6b36b3485e0cb3a827dc7bc3d2a4812103b06ad3e83b98a1d3d0ee57fb415c836eeb9b4b733ec62e0450f5f116ab996d522102e5efec84d1f8d999ae2fbcbbc98285661719f0441d077907315059644b81b16021027dcbafda32e905315e21cddd05ba230357fbb0a10690feebb07f8dcbb115a8bf2103257229a2c7eeca93a2fcce55a7aaa80752de6ba4dcf0f4b9ba69e340467fc6fe21030268324d149a961521c8ce22d8425739249e4b6807adef35b8de1ea9f538cae72103566f2c5434188d2965dacae74a02a30fdc482e9edcb3dcbeda657f204b42766f2103804a4db83951aa3df64d4e83b77747929504cf0e2fa51d4c55848ba8bfcd6e8321030012b5fbd041142ef898d8c0ef561d6443f4660fe5676bac40ab2d1f759b06172102ff1752edf3820d45e7a8948bf320c360d20189432b7f19120645a64666da406b21020cb6ab9c2fee370ac6c2cb038ff6d7689871733227930e5f007d71632fb17b1f21020d5f088f6fbc4b0549bb7ea079158c5f11fd1699a1cce1e6319d86d84c8fa1082103d19ee37e6bc20bb64f33c3e5ebf7c6bf28bed131f3af7cd40cad888d5c96fe77210319627d12cd8b82bea85c8e8d3db65e0f0193ee9b936566c32be738e5b1db73af21028efff002513af4f0dc33eeb523f05810e401d65aa39ed6d8f40a46f48260eef22103794a94ce84a736513f5857d28f52eb77815c1d4229502619a72e0e3d035212522102b6c09f5c6ef55978f75375e428e18c9ba8226577d6a6d76f23172ac0c1181bb02103ea407eaf0393e4177338bc1b1ff0a6d55251ce0aa03bb258319563c586855026210275cd6aa045d5129243fa52899d349621bcdac80514107c6a079ee690dcce50d921031f93a76aad0c93da0ddf1ce75a57958818e29096fbb30e0dbbf21c09c5a4677421030df2cdc1008b5adfd99e3bd70e16093d0a73146761ef8f436e62e94286246487210325f0e2b046c5dc5527dfb4d934f80d86e6288e39abd2c35a28b9bd5dc0e9c177210281eb13777dfd8f08c41f36467b91c6e352608d19dfa5086897d2bfdcd79c1d842102599ebe9983c4f2670b2a72b1deffc6428290ff199e5233a9bbaeb293b9a39e3921033e87b2c80ea15e977115da52f6a842e4d2698e822e2cf2c8c444204df480d4fd210302c9230f6686694abe2319a71d8dfc486df6d4b5953bb5bebd84840a8927e72d210367d7c92e48016db04daa409b38994a3ddf6352d77dce0914515b1f89c5a6bb2c2102f161cdbd9b0665c29d06c4d5889c601e011c33fc254db94641afd585b9c133d72103b143a248eb039676e3916d9224efeb35f617e6dcc829037ed780eb00c4a00a982103c1350d84476c0455119a277e9afaef293edf71c66d3b4b05f0db11d008a0bb4421037a4b9a5c8e0155cd9a8b6430f51880dde0711bbe6e230180e746bc88b5d49da42103f9933b2adc06544fcb5f4ce51ca247e12c335aeea309cc780efc3a67423ea53e21030553c4f45d2130a7ecf7aaf48c0981a21bf211f6de1b943b107296601d4f54d82103104bddf6dc05e773c3a5c0876167656dc1b25b7493316cfc403f13de778cd6932103ae4d64b4af57c4769232bf8a8f52afb44ebccb999b934c7da5cc9c1ff80c22f721020e953a8e5cc1d1a77ed8b87e8e06c71127b9c798573add69b0d0a81cd323f6ae2102f9d19fdca64dc684d89f700caad389091b2c8cb20783e465d515a65ebfa0c1372102d9d9c6afc62a0b33c10cec52e6e8188dbc5d58a0f81dd9028aa816065a2710ae2102e561189404278fb7d6873772b7ff80c14005ffb7f1accd5d1b173a8cceb4ac7e2103c641f2820e27916d65f1fd204b4c14d9406b217ec5fb5ca94cd34aa0960f329220bcd0222a496b4fd57a32d2df920e77ee0f1196b1b8cfdc94a8a510d4c1fd36fa2010d7fbf15421c21524ee7d2b2683ef1256d9c0851f93d7a8f398b7b7640e7d8e2036793f2edb1219bdbdcc3f7e000d0f49f19c681b27bbb4c2da236a538cf9ad7c20615336343d3ff800f10cb0f77b61f4e1c5cfa0f214f22f415378226cf809dc8e20da70f83bb8581187df6f5fcd05541b4ff1e822de47d5efa8b8a7f149099e9917207a4129fb02be45b9fa5806716eaffc9222a49bdda8c522e1ebf0e1631c95646120317ebacbdbb683afe23e97767af8bdf18ac21fb735c3c913149635026c1971d6207050c525aeba83e33076ef5eddec61cdcab04236a1983edbe7ac6f87987eeac920d118342548e19ba3bb2ae82edfed5d3dc7fbbdff413f33824daff02d4e93fa7720217e8e4ae3497745357053cec46f4a25d66ec74547fa0cbdd7dcb7cb316c0f7f20d20994faba0b6f73ac98c9af6c84a9bf4fc3690c02fb1163488a50250e034ef520994b2888428d0bd768566cfbe335df9c854b8fe256d61a6e8a8ed1eb554ecb3d207a41769d53102eab20f96441b213ae57ffb4dd7f3422b3ef9e61ddfab8aec4bf20637d65676a766e69fa4ce945eb6fe87367df5a3194db1932f9924dd25cd707cb208891af637222e843c97446af984a3806a493db7ed075d4c69230a0617e4eec2620b9c6e6e0b83627a20bb04e7e34db093e4e806ecb260a4628a6c6f9ec03de0c362065d39b3ce66241ee9a93dcf69415cc699e237556f5faa71396b1e94092166654209ee2564e0219c84528f622f8637b50fba44d6390473f292bb60f6c9608feb8c5205115a3f3c68653c3e45e65d596596f7b41dec5cd16aaf1ab5f37b277b014776420c5169d3c6e7b710197421561b8adb6fb38423b1dca0b0e35afed25cb6673ce4820fa1a9b76370dc131f1a7bd139c1eaff23316d837025e3ebc0afb858b2d96447b2049af94c8ec645ecddc4c44307d385cae720dfcd0f6428681199b7df540bea64e202e450b019174cf80235148d36578d2ce58f7b154fb62d94e55692839f1cc576a20b61b488fadbb29acbfb4164479838617f7b7700aa774e623c793cbfe439a109a2009ccae61765466d33b4977338644a842877aed608e022fbad8acde8fe23cbc0f20d9bdb6534fcc6348d3c78381bd8c35c8c97a67e61da0ada91213fe9bf1cdf644208f45757b60bb24ebc9c9e3fcf7ccff53ab4aaaa7a0c7d8643a961cf20442d48f20f558b96cbec83ca359df907d31b8299a097f716563c517b5a52604d3eea2296a20b0f259d4fa6221814f3f2a680496952a4344e6dbe7837596dba43f24426c735d2025a4954f77963447a9f316f346f3f11256554f205a8fe57c3b299a89019de962200554f176013b53992f01fb13a24b79784da87edd7af6ceb9ce7e7519b3064db820e1e645b569f31c35b6eeab199e5fdcd4dc4c314e37be088a0e48fff8b90ae94b20ebaf49eb10439be7abd1a8faf6bf5de3d5f634d325a18b4f3205bbbdfe127b1c20b00a5783b9e2cc3aa3caec8557767a2769f17c6dd8292b999f1a4db98ffd130c20b29b2b5c5d1751af5ec0a4374346c25187a60938c84d6f4bbad6251022c8145120c3cab11cb78c5290f67812d9b594d92c8671288a2f5ee8ec51795438afef801d2015939adc4263739d7dc12a3b195f65618baf540cac4a11da70fc3534f6a94852202bc6d6bd6d08cd24a4b935c82b8e860c142c29ebe14dbe38337d96ce1da024f620a17640a035abc5aff948af7292b106a49c82f8d0e018d47197147f558345c24e2024efdbf28af2d3c60cd072afd0733a2a8b2e100fc5fe209fbc5265364a6c1c0820fbd3485a59776a8226f8443bb7bdaed61d03737eeef180e73f64f4951336ec1b201fbe5a8ca567fbcafabebaee558bc2808b820504a81d6ba1f42852773cccb08a20bd3ee509ff904523ad45338b4d636d7e48d30da2864c8c480e5e2a321c69400220353e8d687c750601a1e1fb3f699fe84d7e232a972b3378ff3e9de39c013cf457206687cfe27924da0b02c9f1d1e8a5987f1db15d67e790502d2573afff730bd8862030e43363de46943c753cab66acb393b0386e5d4855f9a29b6bcc9e804a5ca9432009114c8d13b3e1baa14d3e9e8b9c9b0e05a01af12bd831286b7f00173afe0b3520eb6f5d070a995f84056fb962580151c5266d492d4f8372a591f355d849757c94203010aa80bea91ee7402d1f619d065f154b75e601ea74a62a91f4f8ecb2cc98f220ec43fe7c60c36f254585db7173a56a02f13d31affc83db811de2b4f445f88f2a20b6e02be85b8133b69d07436cb2842e8f70c0d2d50d74147a18f6385e22508dda20545b0bd3520fc54fc01e5995c9b2c97c10c296ff513ea2792be923c1e98a8acb2041e5c1bf9b746b8ff525a1fa85f21f87e29605c30972e9c2debda1e0982c037120ebac1818a0f10775815d78a66bd418f33a2c97469dc457a33e18ba8daf093d402086229dbd491efdff2c5556f20f9cd9fab20182caa34e1d63973c89a2f6f343b4201e48b5527f41f18d80ae0be1709eb2e2fd0c0d469285531768a3ea23dd01160220af5e6ea2eafea6c8bd8366264deefa001a673e750ca90ee2deb4640c650b65b620f198b6c7678720346d6d1d2792f8d5b0ed599446517080864b330c4acdfb62312004964f10a37f3412ff12713db244b72830c92c8f70810a11d30c03120cff332d209d8d9b51b149aad4638838664554a07b58121bc773d9729f7cc14260fdc07801202bde17e0c17d7f70c263c9926e80109a725eaf0045526ee1e459da4d5f981a31209548389fd590dc549f288fc6cdfe7957b50d1dbf988c00025f46efcec7be21492038ca4b7e9e7c4105f275b0fefbdf8cc30741b70572ec6df3ebad4e596eb7a03820c194f839ef492c1bd855423ef71ce2731e76beaaca7b16bdda7aa824106fc03920a22f9c31550093e331b38f6576532a4043572988a9023fd4fb3ce059da6ae6d201000100010001000100010001000100010001002103ee9d89508b62c03ca87e431d1db03c09c4fb45d1032ddd8ecc066fabaee1ef9b0100010021037bc2525a58093c52f42a5e335dfe3920baa7cf38c14adaf7e16b1f16ae239eb02102b5f774cd9414bebcbb9eee33c57fd5273749da2d05d336c0ef447e674ecde2802102ece5a6022b9545721ae8b6e13123e1cf5b9dc3b6453891daedc3daccbf4db4862103db5f0a885c23ebcb9fc993ff92917a5ad55eebd32e2a0c08c7c525095d3e35b72103181bae40314955fa2f27b71cd9fab7e081dbe8223d91638881175389bbed6a00010021029796ab0109612f16eb602b3fbb2445e8ef285a2090743eeffdcbefdd9ca2667601000100010001000100010021021ae5149bf80ff59c3309c3776bce60da777c136cab559fa0fa5896f7a7b6ce09010021027f8a8071c7939f1e6159e86df722207649bbe323271fc27888271f6a16f192310100210352b793cd577d3b81b4256cf07ef63431faebe4d700e22dfe5d80f23947faf3af0100010021021f086c123a8325bf8aa5ec4c60ab61077eea2165ada9b4ace0dd31418f29224a010001000100010001000100010001000100010001000100010001000100010001000100010001000100010001000100010001000100010001000100210325bf64a726282a5e9379229b46800b6e7cbdda9c7356d47502043d3d815c806f200000000000000000000000000000000000000000000000000000000000000000030000000200000020ff7304384042e41c9bb79c0d578aa90e997117c537813c16955f1a291f870e9f2028dfc2f7b665413b49abed26a7d6d36f98244cc1a6f718a017869ee7df30ad98206652ba33d2f6d766505ec02a0ad28861825d36afc2a4fd2b994272003084513e20826cd991d154d32e8e3033acc2a5911e60289ad37010e47488c95ad2ff2bcd3c2053ed8fe114ed6403b5335264f15a7d322dcc4f1dfa44499a43c5a36c8558cd172093826fa2911e0cae48a21d126ea0e01392e5048edd8a333b7fd8cdd090ed9c370200000020d395f203e8ceca17db7dab7121048691d42bcde76d0d20b203d9a9e133ca92ec206bb590ef356e2270132f366b4a5e4aaefd561e2e779f5093c00f3139d487a7f2205fc8842c5d2ca598321afc49e0c7a48495ef8fd0175814008c18b09ee7dbc53a03000000010000000000000000000000000000000000000000000000000000000000000000000000ffff0000000000000000000000000000000000000000000000000000000000000000ffff0000000000000000000000000000000000000000000000000000000000000000ffff0120e282ac74bf356a78b28145f08e0b71ab7a15632af75008ab7e1c76ccc753a5c620d488671d6b57962d4f171e71d88a162b5d45c7b7004165f9dcfb0706028ea0db02cb0bb2c62c66f815c91d3cc986395842bd4b4ef295e30ac911303d6059c9357c0103c0198ca5b5778c053737f8435416ea0e42abf0cf3328958f15bf3979af12ab140325bf64a726282a5e9379229b46800b6e7cbdda9c7356d47502043d3d815c806f0000000000000000020217e617f0b6443928278f96999e69a23a4f2c152bdf6d6cdf66e5b80282d4ed00000000";
            else
                str_coin = "820001" +
                           utilityhash_new +
                           "012102d21870f5226f19f7a50ca667c11c0a243d120a0a27d9f3a39a793e720872596321025c6d5a338d4c92a1d0df4cd9b7d9ee67b7273cf3dfedc2870055fa74d091aead2102681ad8bc16f48942b565861f376f88ceedae74f1b8f9c1c70a3c295db7670bfa2103ab471c85a9b402d95ed819f38026f71713b3144c2733017d0417afc42ba7a1ca2102ff83eb0a4d459ff2d2e8b9fe3fdd604c5a34926809d16d4ba0ed6e91ffdd0a2d2102ae48af8bd9ef9a712482b20725f6e3a9c8d5e091de2f0ef540914fb5cc9fedfc2103944ec3e30fcb84824b7dc1b1b76609614210d0549c2b67cfe09e0a0eb5f53ce82102bcf0abea435e738e914616971a3d67695bd2e0be6b4277e35f5b6b10947017462103d5b326a216b74a0e37333eeb6f07d63e517fa8d4b38b69dcfffbeba36818a77b210244381dedd468c99805f674a5175eea399682ded0706a265190a40d6033c3f5ca21026daa3353ad1022c10201cd72eb67910a92be468b47a11df0da3ff711ddf33fc62103df2b6152e51a9e3a47c1ff939a338e82c7758a89e73db1d33f1757b3290a5b6e2102600d31243a6aec6deb4231f5419f4f5b26c01c8cb0f21085c56ef19a0d79e8292102d53236198c09f49eb0814e3621fc67cd46672713a0677363f3bb730e986707f921031cd207418d5235165be0b666455e5de41f13381746b7618e2be4ba72576461e721022d09fd0298d15ebb9184e98f463cdc408b2bfba5fbac847b61616804bab4ad1a21023800597b75f156911d1f0b5a3d9878394d53c3ec35e76b1c746270f86828632921032791c1450cba66c75e6010b11019cf03a3873415ddde8f7116d8b59c54e212fd2103af74b6bfb4911fe65cbb52ee5210db6688f737309bb91f5585ee76df86764ed5210333a8912096599ef532ce58b145bcfb08ba8a2c4f1e355af84edce74e50587baf2102eb034a07deecce160adef0c4f2e782ca9fb239284baa0f02edec9107a0b979f42102647cba64012e429eb6211790548194e6bf675e31f75127222097723962c0b9c821037de9266ba267b17f426ddde83baa12348d2425032d88e5a9171523ec3fe906b221023cdf153b9b9bda258b017ba49d6f5e5be4c80e005e5c5821bf2ddb257901201b2102e6e71c9be1457977333ea52316deab959a6b36b3485e0cb3a827dc7bc3d2a4812103b06ad3e83b98a1d3d0ee57fb415c836eeb9b4b733ec62e0450f5f116ab996d522102e5efec84d1f8d999ae2fbcbbc98285661719f0441d077907315059644b81b16021027dcbafda32e905315e21cddd05ba230357fbb0a10690feebb07f8dcbb115a8bf2103257229a2c7eeca93a2fcce55a7aaa80752de6ba4dcf0f4b9ba69e340467fc6fe21030268324d149a961521c8ce22d8425739249e4b6807adef35b8de1ea9f538cae72103566f2c5434188d2965dacae74a02a30fdc482e9edcb3dcbeda657f204b42766f2103804a4db83951aa3df64d4e83b77747929504cf0e2fa51d4c55848ba8bfcd6e8321030012b5fbd041142ef898d8c0ef561d6443f4660fe5676bac40ab2d1f759b06172102ff1752edf3820d45e7a8948bf320c360d20189432b7f19120645a64666da406b21020cb6ab9c2fee370ac6c2cb038ff6d7689871733227930e5f007d71632fb17b1f21020d5f088f6fbc4b0549bb7ea079158c5f11fd1699a1cce1e6319d86d84c8fa1082103d19ee37e6bc20bb64f33c3e5ebf7c6bf28bed131f3af7cd40cad888d5c96fe77210319627d12cd8b82bea85c8e8d3db65e0f0193ee9b936566c32be738e5b1db73af21028efff002513af4f0dc33eeb523f05810e401d65aa39ed6d8f40a46f48260eef22103794a94ce84a736513f5857d28f52eb77815c1d4229502619a72e0e3d035212522102b6c09f5c6ef55978f75375e428e18c9ba8226577d6a6d76f23172ac0c1181bb02103ea407eaf0393e4177338bc1b1ff0a6d55251ce0aa03bb258319563c586855026210275cd6aa045d5129243fa52899d349621bcdac80514107c6a079ee690dcce50d921031f93a76aad0c93da0ddf1ce75a57958818e29096fbb30e0dbbf21c09c5a4677421030df2cdc1008b5adfd99e3bd70e16093d0a73146761ef8f436e62e94286246487210325f0e2b046c5dc5527dfb4d934f80d86e6288e39abd2c35a28b9bd5dc0e9c177210281eb13777dfd8f08c41f36467b91c6e352608d19dfa5086897d2bfdcd79c1d842102599ebe9983c4f2670b2a72b1deffc6428290ff199e5233a9bbaeb293b9a39e3921033e87b2c80ea15e977115da52f6a842e4d2698e822e2cf2c8c444204df480d4fd210302c9230f6686694abe2319a71d8dfc486df6d4b5953bb5bebd84840a8927e72d210367d7c92e48016db04daa409b38994a3ddf6352d77dce0914515b1f89c5a6bb2c2102f161cdbd9b0665c29d06c4d5889c601e011c33fc254db94641afd585b9c133d72103b143a248eb039676e3916d9224efeb35f617e6dcc829037ed780eb00c4a00a982103c1350d84476c0455119a277e9afaef293edf71c66d3b4b05f0db11d008a0bb4421037a4b9a5c8e0155cd9a8b6430f51880dde0711bbe6e230180e746bc88b5d49da42103f9933b2adc06544fcb5f4ce51ca247e12c335aeea309cc780efc3a67423ea53e21030553c4f45d2130a7ecf7aaf48c0981a21bf211f6de1b943b107296601d4f54d82103104bddf6dc05e773c3a5c0876167656dc1b25b7493316cfc403f13de778cd6932103ae4d64b4af57c4769232bf8a8f52afb44ebccb999b934c7da5cc9c1ff80c22f721020e953a8e5cc1d1a77ed8b87e8e06c71127b9c798573add69b0d0a81cd323f6ae2102f9d19fdca64dc684d89f700caad389091b2c8cb20783e465d515a65ebfa0c1372102d9d9c6afc62a0b33c10cec52e6e8188dbc5d58a0f81dd9028aa816065a2710ae2102e561189404278fb7d6873772b7ff80c14005ffb7f1accd5d1b173a8cceb4ac7e2103c641f2820e27916d65f1fd204b4c14d9406b217ec5fb5ca94cd34aa0960f329220bcd0222a496b4fd57a32d2df920e77ee0f1196b1b8cfdc94a8a510d4c1fd36fa2010d7fbf15421c21524ee7d2b2683ef1256d9c0851f93d7a8f398b7b7640e7d8e2036793f2edb1219bdbdcc3f7e000d0f49f19c681b27bbb4c2da236a538cf9ad7c20615336343d3ff800f10cb0f77b61f4e1c5cfa0f214f22f415378226cf809dc8e20da70f83bb8581187df6f5fcd05541b4ff1e822de47d5efa8b8a7f149099e9917207a4129fb02be45b9fa5806716eaffc9222a49bdda8c522e1ebf0e1631c95646120317ebacbdbb683afe23e97767af8bdf18ac21fb735c3c913149635026c1971d6207050c525aeba83e33076ef5eddec61cdcab04236a1983edbe7ac6f87987eeac920d118342548e19ba3bb2ae82edfed5d3dc7fbbdff413f33824daff02d4e93fa7720217e8e4ae3497745357053cec46f4a25d66ec74547fa0cbdd7dcb7cb316c0f7f20d20994faba0b6f73ac98c9af6c84a9bf4fc3690c02fb1163488a50250e034ef520994b2888428d0bd768566cfbe335df9c854b8fe256d61a6e8a8ed1eb554ecb3d207a41769d53102eab20f96441b213ae57ffb4dd7f3422b3ef9e61ddfab8aec4bf20637d65676a766e69fa4ce945eb6fe87367df5a3194db1932f9924dd25cd707cb208891af637222e843c97446af984a3806a493db7ed075d4c69230a0617e4eec2620b9c6e6e0b83627a20bb04e7e34db093e4e806ecb260a4628a6c6f9ec03de0c362065d39b3ce66241ee9a93dcf69415cc699e237556f5faa71396b1e94092166654209ee2564e0219c84528f622f8637b50fba44d6390473f292bb60f6c9608feb8c5205115a3f3c68653c3e45e65d596596f7b41dec5cd16aaf1ab5f37b277b014776420c5169d3c6e7b710197421561b8adb6fb38423b1dca0b0e35afed25cb6673ce4820fa1a9b76370dc131f1a7bd139c1eaff23316d837025e3ebc0afb858b2d96447b2049af94c8ec645ecddc4c44307d385cae720dfcd0f6428681199b7df540bea64e202e450b019174cf80235148d36578d2ce58f7b154fb62d94e55692839f1cc576a20b61b488fadbb29acbfb4164479838617f7b7700aa774e623c793cbfe439a109a2009ccae61765466d33b4977338644a842877aed608e022fbad8acde8fe23cbc0f20d9bdb6534fcc6348d3c78381bd8c35c8c97a67e61da0ada91213fe9bf1cdf644208f45757b60bb24ebc9c9e3fcf7ccff53ab4aaaa7a0c7d8643a961cf20442d48f20f558b96cbec83ca359df907d31b8299a097f716563c517b5a52604d3eea2296a20b0f259d4fa6221814f3f2a680496952a4344e6dbe7837596dba43f24426c735d2025a4954f77963447a9f316f346f3f11256554f205a8fe57c3b299a89019de962200554f176013b53992f01fb13a24b79784da87edd7af6ceb9ce7e7519b3064db820e1e645b569f31c35b6eeab199e5fdcd4dc4c314e37be088a0e48fff8b90ae94b20ebaf49eb10439be7abd1a8faf6bf5de3d5f634d325a18b4f3205bbbdfe127b1c20b00a5783b9e2cc3aa3caec8557767a2769f17c6dd8292b999f1a4db98ffd130c20b29b2b5c5d1751af5ec0a4374346c25187a60938c84d6f4bbad6251022c8145120c3cab11cb78c5290f67812d9b594d92c8671288a2f5ee8ec51795438afef801d2015939adc4263739d7dc12a3b195f65618baf540cac4a11da70fc3534f6a94852202bc6d6bd6d08cd24a4b935c82b8e860c142c29ebe14dbe38337d96ce1da024f620a17640a035abc5aff948af7292b106a49c82f8d0e018d47197147f558345c24e2024efdbf28af2d3c60cd072afd0733a2a8b2e100fc5fe209fbc5265364a6c1c0820fbd3485a59776a8226f8443bb7bdaed61d03737eeef180e73f64f4951336ec1b201fbe5a8ca567fbcafabebaee558bc2808b820504a81d6ba1f42852773cccb08a20bd3ee509ff904523ad45338b4d636d7e48d30da2864c8c480e5e2a321c69400220353e8d687c750601a1e1fb3f699fe84d7e232a972b3378ff3e9de39c013cf457206687cfe27924da0b02c9f1d1e8a5987f1db15d67e790502d2573afff730bd8862030e43363de46943c753cab66acb393b0386e5d4855f9a29b6bcc9e804a5ca9432009114c8d13b3e1baa14d3e9e8b9c9b0e05a01af12bd831286b7f00173afe0b3520eb6f5d070a995f84056fb962580151c5266d492d4f8372a591f355d849757c94203010aa80bea91ee7402d1f619d065f154b75e601ea74a62a91f4f8ecb2cc98f220ec43fe7c60c36f254585db7173a56a02f13d31affc83db811de2b4f445f88f2a20b6e02be85b8133b69d07436cb2842e8f70c0d2d50d74147a18f6385e22508dda20545b0bd3520fc54fc01e5995c9b2c97c10c296ff513ea2792be923c1e98a8acb2041e5c1bf9b746b8ff525a1fa85f21f87e29605c30972e9c2debda1e0982c037120ebac1818a0f10775815d78a66bd418f33a2c97469dc457a33e18ba8daf093d402086229dbd491efdff2c5556f20f9cd9fab20182caa34e1d63973c89a2f6f343b4201e48b5527f41f18d80ae0be1709eb2e2fd0c0d469285531768a3ea23dd01160220af5e6ea2eafea6c8bd8366264deefa001a673e750ca90ee2deb4640c650b65b620f198b6c7678720346d6d1d2792f8d5b0ed599446517080864b330c4acdfb62312004964f10a37f3412ff12713db244b72830c92c8f70810a11d30c03120cff332d209d8d9b51b149aad4638838664554a07b58121bc773d9729f7cc14260fdc07801202bde17e0c17d7f70c263c9926e80109a725eaf0045526ee1e459da4d5f981a31209548389fd590dc549f288fc6cdfe7957b50d1dbf988c00025f46efcec7be21492038ca4b7e9e7c4105f275b0fefbdf8cc30741b70572ec6df3ebad4e596eb7a03820c194f839ef492c1bd855423ef71ce2731e76beaaca7b16bdda7aa824106fc03920a22f9c31550093e331b38f6576532a4043572988a9023fd4fb3ce059da6ae6d201000100010001000100010001000100010001002103ee9d89508b62c03ca87e431d1db03c09c4fb45d1032ddd8ecc066fabaee1ef9b0100010021037bc2525a58093c52f42a5e335dfe3920baa7cf38c14adaf7e16b1f16ae239eb02102b5f774cd9414bebcbb9eee33c57fd5273749da2d05d336c0ef447e674ecde2802102ece5a6022b9545721ae8b6e13123e1cf5b9dc3b6453891daedc3daccbf4db4862103db5f0a885c23ebcb9fc993ff92917a5ad55eebd32e2a0c08c7c525095d3e35b72103181bae40314955fa2f27b71cd9fab7e081dbe8223d91638881175389bbed6a00010021029796ab0109612f16eb602b3fbb2445e8ef285a2090743eeffdcbefdd9ca2667601000100010001000100010021021ae5149bf80ff59c3309c3776bce60da777c136cab559fa0fa5896f7a7b6ce09010021027f8a8071c7939f1e6159e86df722207649bbe323271fc27888271f6a16f192310100210352b793cd577d3b81b4256cf07ef63431faebe4d700e22dfe5d80f23947faf3af0100010021021f086c123a8325bf8aa5ec4c60ab61077eea2165ada9b4ace0dd31418f29224a010001000100010001000100010001000100010001000100010001000100010001000100010001000100010001000100010001000100010001000100210325bf64a726282a5e9379229b46800b6e7cbdda9c7356d47502043d3d815c806f200000000000000000000000000000000000000000000000000000000000000000030000000200000020ff7304384042e41c9bb79c0d578aa90e997117c537813c16955f1a291f870e9f2028dfc2f7b665413b49abed26a7d6d36f98244cc1a6f718a017869ee7df30ad98206652ba33d2f6d766505ec02a0ad28861825d36afc2a4fd2b994272003084513e20826cd991d154d32e8e3033acc2a5911e60289ad37010e47488c95ad2ff2bcd3c2053ed8fe114ed6403b5335264f15a7d322dcc4f1dfa44499a43c5a36c8558cd172093826fa2911e0cae48a21d126ea0e01392e5048edd8a333b7fd8cdd090ed9c370200000020d395f203e8ceca17db7dab7121048691d42bcde76d0d20b203d9a9e133ca92ec206bb590ef356e2270132f366b4a5e4aaefd561e2e779f5093c00f3139d487a7f2205fc8842c5d2ca598321afc49e0c7a48495ef8fd0175814008c18b09ee7dbc53a03000000010000000000000000000000000000000000000000000000000000000000000000000000ffff0000000000000000000000000000000000000000000000000000000000000000ffff0000000000000000000000000000000000000000000000000000000000000000ffff0120e282ac74bf356a78b28145f08e0b71ab7a15632af75008ab7e1c76ccc753a5c620d488671d6b57962d4f171e71d88a162b5d45c7b7004165f9dcfb0706028ea0db02cb0bb2c62c66f815c91d3cc986395842bd4b4ef295e30ac911303d6059c9357c0103c0198ca5b5778c053737f8435416ea0e42abf0cf3328958f15bf3979af12ab140325bf64a726282a5e9379229b46800b6e7cbdda9c7356d47502043d3d815c806f0000000000000000020217e617f0b6443928278f96999e69a23a4f2c152bdf6d6cdf66e5b80282d4ed00000000";
            byte[] byStr = str_coin.HexToBytes();
            RingConfidentialTransaction tx = byStr.AsSerializable<RingConfidentialTransaction>();

            return tx;
        }

        public static Transaction[] GetGenesisInitialTxs()
        {
            if (IsTestRingCT == true)
            {
                return new Transaction[]
                {
                    new MinerTransaction
                    {
                        Nonce = 2083236893,
                        Attributes = new TransactionAttribute[0],
                        Inputs = new CoinReference[0],
                        Outputs = new TransactionOutput[0],
                        Scripts = new Witness[0]
                    },
                    GoverningToken,
                    UtilityToken,
                    new IssueTransaction
                    {
                        Attributes = new TransactionAttribute[0],
                        Inputs = new CoinReference[0],
                        Outputs = new[]
                        {
                            new TransactionOutput
                            {
                                AssetId = GoverningToken.Hash,
                                Value = GoverningToken.Amount,
                                ScriptHash = Contract.CreateMultiSigRedeemScript(StandbyValidators.Length / 2 + 1, StandbyValidators).ToScriptHash()
                            },
                            new TransactionOutput
                            {
                                AssetId = UtilityToken.Hash,
                                Value = Fixed8.FromDecimal(88888888),
                                ScriptHash = Contract.CreateMultiSigRedeemScript(StandbyValidators.Length / 2 + 1, StandbyValidators).ToScriptHash()
                            }
                        },
                        Scripts = new[]
                        {
                            new Witness
                            {
                                InvocationScript = new byte[0],
                                VerificationScript = new[] { (byte)OpCode.PUSHT }
                            }
                        }
                    },
                    GetRingConfidentialTransaction(),
                    GetRingConfidentialTransaction(1)
                };
            }
            else
            {
                return new Transaction[]
                {
                    new MinerTransaction
                    {
                        Nonce = 2083236893,
                        Attributes = new TransactionAttribute[0],
                        Inputs = new CoinReference[0],
                        Outputs = new TransactionOutput[0],
                        Scripts = new Witness[0]
                    },
                    GoverningToken,
                    UtilityToken,
                    new IssueTransaction
                    {
                        Attributes = new TransactionAttribute[0],
                        Inputs = new CoinReference[0],
                        Outputs = new[]
                        {
                            new TransactionOutput
                            {
                                AssetId = GoverningToken.Hash,
                                Value = GoverningToken.Amount,
                                ScriptHash = Contract.CreateMultiSigRedeemScript(StandbyValidators.Length / 2 + 1, StandbyValidators).ToScriptHash()
                            },
                            new TransactionOutput
                            {
                                AssetId = UtilityToken.Hash,
                                Value = UtilityToken.Amount,
                                ScriptHash = Contract.CreateMultiSigRedeemScript(StandbyValidators.Length / 2 + 1, StandbyValidators).ToScriptHash()
                            }
                        },
                        Scripts = new[]
                        {
                            new Witness
                            {
                                InvocationScript = new byte[0],
                                VerificationScript = new[] { (byte)OpCode.PUSHT }
                            }
                        }
                    }
                };
            }
        }

        public static readonly Block GenesisBlock = new Block
        {
            PrevHash = UInt256.Zero,
            Timestamp = (new DateTime(2018, 4, 1, 15, 35, 21, DateTimeKind.Utc)).ToTimestamp(),
            Index = 0,
            ConsensusData = 2083236893,
            NextConsensus = GetConsensusAddress(StandbyValidators),
            Script = new Witness
            {
                InvocationScript = new byte[0],
                VerificationScript = new[] { (byte)OpCode.PUSHT }
            },
            Transactions = GetGenesisInitialTxs()
        };

        public abstract UInt256 CurrentBlockHash { get; }

        public abstract UInt256 CurrentHeaderHash { get; }

        public static Blockchain Default { get; private set; } = null;

        public abstract uint HeaderHeight { get; }

        public abstract uint Height { get; }

        static Blockchain()
        {
            GenesisBlock.RebuildMerkleRoot();
        }

        public abstract bool AddBlock(Block block);

        protected internal abstract void AddHeaders(IEnumerable<Header> headers);

        public static Fixed8 CalculateBonus(IEnumerable<CoinReference> inputs, bool ignoreClaimed = true)
        {
            List<SpentCoin> unclaimed = new List<SpentCoin>();
            foreach (var group in inputs.GroupBy(p => p.PrevHash))
            {
                Dictionary<ushort, SpentCoin> claimable = Default.GetUnclaimed(group.Key);
                if (claimable == null || claimable.Count == 0)
                    if (ignoreClaimed)
                        continue;
                    else
                        throw new ArgumentException();
                foreach (CoinReference claim in group)
                {
                    if (!claimable.TryGetValue(claim.PrevIndex, out SpentCoin claimed))
                        if (ignoreClaimed)
                            continue;
                        else
                            throw new ArgumentException();
                    unclaimed.Add(claimed);
                }
            }
            return CalculateBonusInternal(unclaimed);
        }

        public static Fixed8 CalculateBonus(IEnumerable<CoinReference> inputs, uint height_end)
        {
            List<SpentCoin> unclaimed = new List<SpentCoin>();
            foreach (var group in inputs.GroupBy(p => p.PrevHash))
            {
                Transaction tx = Default.GetTransaction(group.Key, out int height_start);
                if (tx == null) throw new ArgumentException();
                if (height_start == height_end) continue;
                foreach (CoinReference claim in group)
                {
                    if (claim.PrevIndex >= tx.Outputs.Length || !tx.Outputs[claim.PrevIndex].AssetId.Equals(GoverningToken.Hash))
                        throw new ArgumentException();
                    unclaimed.Add(new SpentCoin
                    {
                        Output = tx.Outputs[claim.PrevIndex],
                        StartHeight = (uint)height_start,
                        EndHeight = height_end
                    });
                }
            }
            return CalculateBonusInternal(unclaimed);
        }

        private static Fixed8 CalculateBonusInternal(IEnumerable<SpentCoin> unclaimed)
        {
            Fixed8 amount_claimed = Fixed8.Zero;
            foreach (var group in unclaimed.GroupBy(p => new { p.StartHeight, p.EndHeight }))
            {
                uint amount = 0;
                uint ustart = group.Key.StartHeight / DecrementInterval;
                if (ustart < GenerationAmount.Length)
                {
                    uint istart = group.Key.StartHeight % DecrementInterval;
                    uint uend = group.Key.EndHeight / DecrementInterval;
                    uint iend = group.Key.EndHeight % DecrementInterval;
                    if (uend >= GenerationAmount.Length)
                    {
                        uend = (uint)GenerationAmount.Length;
                        iend = 0;
                    }
                    if (iend == 0)
                    {
                        uend--;
                        iend = DecrementInterval;
                    }
                    while (ustart < uend)
                    {
                        amount += (DecrementInterval - istart) * GenerationAmount[ustart];
                        ustart++;
                        istart = 0;
                    }
                    amount += (iend - istart) * GenerationAmount[ustart];
                }
                amount += (uint)(Default.GetSysFeeAmount(group.Key.EndHeight - 1) - (group.Key.StartHeight == 0 ? 0 : Default.GetSysFeeAmount(group.Key.StartHeight - 1)));
                amount_claimed += group.Sum(p => p.Value) / 100000000 * amount;
            }
            return amount_claimed;
        }

        public abstract bool ContainsBlock(UInt256 hash);

        public abstract bool ContainsTransaction(UInt256 hash);

        public bool ContainsUnspent(CoinReference input)
        {
            return ContainsUnspent(input.PrevHash, input.PrevIndex);
        }

        public abstract bool ContainsUnspent(UInt256 hash, ushort index);

        public abstract DataCache<TKey, TValue> CreateCache<TKey, TValue>()
            where TKey : IEquatable<TKey>, ISerializable, new()
            where TValue : class, ISerializable, new();

        public abstract void Dispose();

        public abstract AccountState GetAccountState(UInt160 script_hash);

        public abstract AssetState GetAssetState(UInt256 asset_id);

        public Block GetBlock(uint height)
        {
            return GetBlock(GetBlockHash(height));
        }

        public abstract Block GetBlock(UInt256 hash);

        public abstract UInt256 GetBlockHash(uint height);

        public abstract ContractState GetContract(UInt160 hash);

        public abstract IEnumerable<ValidatorState> GetEnrollments();

        public abstract Header GetHeader(uint height);

        public abstract Header GetHeader(UInt256 hash);

        public static UInt160 GetConsensusAddress(ECPoint[] validators)
        {
            return Contract.CreateMultiSigRedeemScript(validators.Length - (validators.Length - 1) / 3, validators).ToScriptHash();
        }

        private List<ECPoint> _validators = new List<ECPoint>();

        public ECPoint[] GetValidators()
        {
            lock (_validators)
            {
                if (_validators.Count == 0)
                {
                    _validators.AddRange(GetValidators(Enumerable.Empty<Transaction>()));
                }
                return _validators.ToArray();
            }
        }

        public virtual IEnumerable<ECPoint> GetValidators(IEnumerable<Transaction> others)
        {
            VoteState[] votes = GetVotes(others).OrderBy(p => p.PublicKeys.Length).ToArray();
            int validators_count = (int)votes.WeightedFilter(0.25, 0.75, p => p.Count.GetData(), (p, w) => new
            {
                ValidatorsCount = p.PublicKeys.Length,
                Weight = w
            }).WeightedAverage(p => p.ValidatorsCount, p => p.Weight);
            validators_count = Math.Max(validators_count, StandbyValidators.Length);
            Dictionary<ECPoint, Fixed8> validators = GetEnrollments().ToDictionary(p => p.PublicKey, p => Fixed8.Zero);
            foreach (var vote in votes)
            {
                foreach (ECPoint pubkey in vote.PublicKeys.Take(validators_count))
                {
                    if (validators.ContainsKey(pubkey))
                        validators[pubkey] += vote.Count;
                }
            }
            return validators.OrderByDescending(p => p.Value).ThenBy(p => p.Key).Select(p => p.Key).Take(validators_count);
        }

        public abstract Block GetNextBlock(UInt256 hash);

        public abstract UInt256 GetNextBlockHash(UInt256 hash);

        byte[] IScriptTable.GetScript(byte[] script_hash)
        {
            return GetContract(new UInt160(script_hash)).Script;
        }

        public abstract StorageItem GetStorageItem(StorageKey key);

        public virtual long GetSysFeeAmount(uint height)
        {
            return GetSysFeeAmount(GetBlockHash(height));
        }

        public abstract long GetSysFeeAmount(UInt256 hash);

        public Transaction GetTransaction(UInt256 hash)
        {
            return GetTransaction(hash, out _);
        }

        public abstract Transaction GetTransaction(UInt256 hash, out int height);

        public abstract Dictionary<ushort, SpentCoin> GetUnclaimed(UInt256 hash);

        public abstract TransactionOutput GetUnspent(UInt256 hash, ushort index);
        public abstract IntPtr GetCmMerkleTree();

        public IEnumerable<VoteState> GetVotes()
        {
            return GetVotes(Enumerable.Empty<Transaction>());
        }

        public abstract IEnumerable<VoteState> GetVotes(IEnumerable<Transaction> others);

        public abstract bool IsDoubleSpend(Transaction tx);

        public abstract bool IsDoubleNullifier(Transaction tx);

        public abstract bool IsNullifierAdded(UInt256 nullifier);

        public abstract bool IsRingCTCommitmentAdded(UInt256 assetID, byte[] keyImage);

        public abstract bool IsDoubleRingCTCommitment(Transaction tx);

        protected void OnNotify(Block block, NotifyEventArgs[] notifications)
        {
            Notify?.Invoke(this, new BlockNotifyEventArgs(block, notifications));
        }

        protected void OnPersistCompleted(Block block)
        {
            lock (_validators)
            {
                _validators.Clear();
            }
            PersistCompleted?.Invoke(this, block);
        }

        public static Blockchain RegisterBlockchain(Blockchain blockchain)
        {
            if (Default != null) Default.Dispose();
            Default = blockchain ?? throw new ArgumentNullException();
            return blockchain;
        }
    }
}
