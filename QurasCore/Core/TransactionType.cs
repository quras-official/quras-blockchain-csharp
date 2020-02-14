#pragma warning disable CS0612

using Quras.IO.Caching;

namespace Quras.Core
{ 
    public enum TransactionType : byte
    {
        [ReflectionCache(typeof(MinerTransaction))]
        MinerTransaction = 0x00,

        [ReflectionCache(typeof(IssueTransaction))]
        IssueTransaction = 0x01,

        [ReflectionCache(typeof(ClaimTransaction))]
        ClaimTransaction = 0x02,

        [ReflectionCache(typeof(EnrollmentTransaction))]
        EnrollmentTransaction = 0x20,

        [ReflectionCache(typeof(RegisterTransaction))]
        RegisterTransaction = 0x40,

        [ReflectionCache(typeof(ContractTransaction))]
        ContractTransaction = 0x80,

        [ReflectionCache(typeof(AnonymousContractTransaction))]
        AnonymousContractTransaction = 0x81,

        [ReflectionCache(typeof(RingConfidentialTransaction))]
        RingConfidentialTransaction = 0x82,

        [ReflectionCache(typeof(PublishTransaction))]
        PublishTransaction = 0xd0,

        [ReflectionCache(typeof(InvocationTransaction))]
        InvocationTransaction = 0xd1,

        [ReflectionCache(typeof(UploadRequestTransaction))]
        UploadRequestTransaction = 0xe0,

        [ReflectionCache(typeof(DownloadRequestTransaction))]
        DownloadRequestTransaction = 0xe1,

        [ReflectionCache(typeof(ApproveDownloadTransaction))]
        ApproveDownloadTransaction = 0xe2,

        [ReflectionCache(typeof(PayFileTransaction))]
        PayFileTransaction = 0xe4

    }
}
