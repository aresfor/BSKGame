using System;
using System.Collections.Generic;
using Game.Gameplay;

namespace QFramework
{
    internal interface IPackageManagerServer : IModel
    {
        void DeletePackage(string packageId, System.Action onResponse);
        void GetAllRemotePackageInfoV5(Action<List<PackageRepository>, List<string>> onResponse);
    }
}