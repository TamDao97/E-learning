using Elearning.Model.Models.InitData;
using Elearning.Models.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Elearning.Api.AppInitialize
{
    public class InitPermission
    {
        public static void Init (ElearningContext _sqlContext)
        {
            string pathGroupFunction = Path.Combine(Directory.GetCurrentDirectory(), "InitData/GroupFunction.json");
            if (File.Exists(pathGroupFunction))
            {
                StreamReader stream = new StreamReader(pathGroupFunction);
                string json = stream.ReadToEnd();
                var groupFunctions = JsonConvert.DeserializeObject<StaticPermission>(File.ReadAllText(pathGroupFunction));
                //StaticPermission groupFunctions = JsonConvert.DeserializeObject<StaticPermission>(json);

                var listGroup = _sqlContext.GroupFunction.ToList();
                bool isExist = false;
                List<Permission> listFunction;
                GroupFunction group;
                StaticGroupFunction groupFunction;
                StaticFuncfion function;
                UserPermission user;
               
                foreach (var groupF in listGroup)
                {
                    isExist = false;
                    listFunction = _sqlContext.Permission.Where(r => r.GroupFunctionId == groupF.Id).ToList();
                    groupFunction = groupFunctions.Groups.FirstOrDefault(r => groupF.Id.Equals(r.Id));
                    //foreach (var item in listFunction)
                    //{
                    //    user = new UserPermission()
                    //    {
                    //        Id = Guid.NewGuid().ToString(),
                    //        UserId = "USFIX000001",
                    //        PermissionId=item.Id,
                    //    };
                    //    _sqlContext.UserPermission.Add(user);
                    //}
                    if (groupFunction != null)
                    {
                        foreach (var func in listFunction)
                        {
                            function = groupFunctions.Functions.FirstOrDefault(r => func.Id.Equals(r.Id));
                            if (function == null)
                            {
                                _sqlContext.UserPermission.RemoveRange(_sqlContext.UserPermission.Where(r => r.PermissionId.Equals(func.Id)).ToList());

                                _sqlContext.SaveChanges();
                                _sqlContext.GroupPermission.RemoveRange(_sqlContext.GroupPermission.Where(r => r.PermissionId.Equals(func.Id)).ToList());

                                _sqlContext.SaveChanges();
                                _sqlContext.Permission.Remove(_sqlContext.Permission.FirstOrDefault(r => r.Id.Equals(func.Id)));
                            }
                        }
                    }
                    else
                    {

                        _sqlContext.UserPermission.RemoveRange(from p in _sqlContext.Permission
                                                              join u in _sqlContext.UserPermission on p.Id equals u.PermissionId
                                                              where p.GroupFunctionId.Equals(groupF.Id)
                                                              select u);

                        _sqlContext.GroupPermission.RemoveRange(from p in _sqlContext.Permission
                                                               join u in _sqlContext.GroupPermission on p.Id equals u.PermissionId
                                                               where p.GroupFunctionId.Equals(groupF.Id)
                                                               select u);

                        _sqlContext.SaveChanges();
                        _sqlContext.Permission.RemoveRange(from p in _sqlContext.Permission
                                                          where p.GroupFunctionId.Equals(groupF.Id)
                                                          select p);

                        _sqlContext.SaveChanges();
                        _sqlContext.GroupFunction.RemoveRange(from p in _sqlContext.GroupFunction
                                                             where p.Id.Equals(groupF.Id)
                                                             select p);
                    }
                }

                _sqlContext.SaveChanges();

                foreach (var groupf in groupFunctions.Groups)
                {
                    group = _sqlContext.GroupFunction.FirstOrDefault(r => r.Id.Equals(groupf.Id));

                    if (group == null)
                    {
                        group = new GroupFunction
                        {
                            Name = groupf.Name,
                            Code = groupf.Code,
                            Index = groupf.Index,
                            Id = groupf.Id
                        };

                        _sqlContext.GroupFunction.Add(group);

                    }
                    else
                    {
                        group.Name = groupf.Name;
                        group.Code = groupf.Code;
                        group.Index = groupf.Index;
                    }
                }

                _sqlContext.SaveChanges();

                Permission permission;
                foreach (var fun in groupFunctions.Functions)
                {
                    permission = _sqlContext.Permission.FirstOrDefault(r => r.Id.Equals(fun.Id));
                    if (permission == null)
                    {
                        permission = new Permission
                        {
                            Code = fun.Code,
                            Id = fun.Id,
                            GroupFunctionId = fun.GroupId,
                            Name = fun.Name,
                            ScreenCode = fun.ScreenCode,
                            //Index = fun.Index
                        };

                        _sqlContext.Permission.Add(permission);
                    }
                    else
                    {
                        permission.Name = fun.Name;
                        permission.Code = fun.Code;
                        permission.ScreenCode = fun.ScreenCode;
                        permission.GroupFunctionId = fun.GroupId;
                        //permission.Index = fun.Index;
                    }
                }

                _sqlContext.SaveChanges();

            }
        }
    }
}
