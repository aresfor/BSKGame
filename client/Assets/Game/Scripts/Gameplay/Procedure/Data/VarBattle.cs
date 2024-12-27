//
//
// using Game.Gameplay;
// using GameFramework;
//
// namespace Game.Client
// {
//     /// <summary>
//     /// System.Int32 变量类。
//     /// </summary>
//     public sealed class VarBattle : Variable<DRBattle>
//     {
//         /// <summary>
//         /// 初始化 System.Int32 变量类的新实例。
//         /// </summary>
//         public VarBattle()
//         {
//         }
//
//         /// <summary>
//         /// 从 System.Int32 到 System.Int32 变量类的隐式转换。
//         /// </summary>
//         /// <param name="value">值。</param>
//         public static implicit operator VarBattle(DRBattle value)
//         {
//             VarBattle varValue = ReferencePool.Acquire<VarBattle>();
//             varValue.Value = value;
//             return varValue;
//         }
//
//         /// <summary>
//         /// 从 System.Int32 变量类到 System.Int32 的隐式转换。
//         /// </summary>
//         /// <param name="value">值。</param>
//         public static implicit operator DRBattle(VarBattle value)
//         {
//             return value.Value;
//         }
//     }
// }