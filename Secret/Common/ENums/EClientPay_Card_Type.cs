namespace HIDE.RestAndRant.Common.ENums
{
    /// <summary>
    /// Тип карты (маэсро, виза, ...) 
    /// </summary>
    public enum EClientPay_Card_Type : int
    {
        /// <summary>
        /// Не установлено
        /// </summary>
        _NotSet = 0,

        /// <summary>
        /// Unknown 
        /// </summary>
        Unknown = 100,

        /// <summary>
        ///  Mastercard и Maestro
        /// </summary>
        MasterCard = 1,

        /// <summary>
        /// Visa и Visa Electron
        /// </summary>
        Visa = 3,

        /// <summary>
        /// Мир
        /// </summary>
        Mir = 5,

        /// <summary>
        /// UnionPay
        /// </summary>
        UnionPay = 7,

        /// <summary>
        /// JCB
        /// </summary>
        JCB = 9,

        /// <summary>
        /// AmericanExpress
        /// </summary>
        AmericanExpress = 11,

        /// <summary>
        /// DinersClub
        /// </summary>
        DinersClub = 13,

        /// <summary>
        /// DiscoverCard
        /// </summary>
        DiscoverCard = 15,

        /// <summary>
        /// InstaPayment
        /// </summary>
        InstaPayment = 17,

        /// <summary>
        /// InstaPaymentTM
        /// </summary>
        InstaPaymentTM = 19,

        /// <summary>
        /// Laser
        /// </summary>
        Laser = 21,

        /// <summary>
        /// Dankort
        /// </summary>
        Dankort = 23,

        /// <summary>
        /// Solo
        /// </summary>
        Solo = 25,

        /// <summary>
        /// Switch 
        /// </summary>
        Switch = 27,

    }
}