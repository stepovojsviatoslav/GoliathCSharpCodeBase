using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class worldgenerator {
 public void init (self);
       this.generator = luanet.LandscapeGenerator();
       this.config = ConfigComponent("worlds","SimpleWorld");
 }})
 public void GetCreateWorldCoroutine(){
   return coroutine.create(function (){
       self:CreateWorld();
   })
 }
 public void CreateWorld(){
   // создаем заготовку для карты
   Console:Message("Initialize world layers...");
   this.generator:AddLayer(luanet.CreateMapLayer(this.config:Get("mapName"), this.config:Get("mapQuadSize"), this.config:Get("mapWorldSize")));
   
   // строим диаграмму Вороного (на основе графа Вороного)
   this.generator:AddLayer(luanet.VoronoiLayer({
     map = this.config:Get("mapName"),;
     min_sites_count = this.config:Get("minSitesCount"),;
     max_sites_count = this.config:Get("maxSitesCount"),;
     relax_iterations = this.config:Get("relaxIterations"),;
     base_radius = this.config:Get("baseRadius"),;
     debug = G_Debug,;
   }))  
   
   // составляем таблицы коэффициентов распределения для групп биомов, параметров групп, общего состава биомов
   var i = 0;
   var groupDistribution = {}  
   var groupNames = this.config:Get("groupName");
   var groupValues = this.config:Get("groupValue");
   var groupParams = {}
   var bioms = {}  
   
   while((i < #groupNames  &&  i < #groupValues) ){ 
     i = i + 1    ;
     table.insert(groupDistribution, {name = groupNames[i], value = groupValues[i]})            
     var currentGroup = GameController.database.data.groups[groupNames[i//
     table.insert(groupParams, currentGroup);
     var j = 0;
     while(j < #currentGroup["bioms"] ){
       j = j + 1;
       if(not bioms[currentGroup["bioms"][j//  ){ 
         bioms[currentGroup["bioms"][j// = GameController.database.data.bioms[currentGroup["bioms"][j//
       }      
     }
   }
   
   // распределяем ячеейки диаграммы Вороного между биомами
   this.generator:AddLayer(luanet.VoronoiMarkBiomsLayer({
     map = this.config:Get("mapName"),;
     groups = groupDistribution,;
     noise_coef = this.config:Get("noiseCoef"),;
     noise_offset = this.config:Get("noiseOffset"),;
     debug = G_Debug,;
   }))
   
   // усекаем размер графа Вороного до заданного
   this.generator:AddLayer(luanet.VoronoiSeparatorLayer({
     map = this.config:Get("mapName"),;
     points_count = this.config:Get("truncPointsCount"),;
     min_distance = this.config:Get("truncMinDistance"),;
     depth = this.config:Get("truncDepth"),;
     debug = G_Debug,;
   }))
   // составляем карту биомов
   this.generator:AddLayer(luanet.VoronoiRasterization());
   
   // составляем карту маркеров (островов ячеек одного биома), карту соседей для граничных точек областей, карту высот
   this.generator:AddLayer(luanet.VoronoiHeightMap());
   
   // составляем список областей связности (в смысле достижимости) и изменяем их высоты для того, чтобы попытаться построить односвязный граф для всех ячеек
   this.generator:AddLayer(luanet.VoronoiLinkZones());
   
   // создаем карту лестниц, указываем направление для каждой лестницы
   this.generator:AddLayer(luanet.VoronoiCreateStairs());
   
   // генерируем тайлы и ди-тайлы, размещаем информацию о них в хранилище игровых объектов (storage)
   this.generator:AddLayer(luanet.VoronoiCubeLayer());
   
   // выполняем настройку мира и назначаем заданные параметры категории мира для каждой позиции карты  
   this.generator:AddLayer(luanet.CreateWorldLayer(bioms, groupParams))   ;
   coroutine.yield();
   var summary_time = 0;
   for(i = 1, this.generator:GetCount() ){
     var start_time = os.clock();
     this.generator:ApplyLayer(i - 1);
     summary_time = summary_time + (os.clock() - start_time);
     Console:Message("Layer " .. i .. " took a: " .. os.clock() - start_time .. " seconds")    ;
     coroutine.yield();
   }
   Console:Message("Summary time: " .. summary_time)  ;
   var scatterCoroutine = coroutine.create(function (){
     self:Scatter();
   })
   var start_time = os.clock();
   while(coroutine.status(scatterCoroutine) != "dead" ){
     coroutine.resume(scatterCoroutine, self);
     coroutine.yield();
   }  
   summary_time = os.clock() - start_time;
   Console:Message("Scatter object during: " .. summary_time .. "s")  ;
   coroutine.yield();
   Console:Message("Init decals  &&  billboards...");
   coroutine.yield();
   luanet.GameFacade.world:InitBillboards();
   luanet.GameFacade.world:InitDecals();  
   //luanet.GameFacade.world:WriteWorldDataToFile()
   Console:Message("World generated.");
 }
 public void LoadWorld(){
   luanet.World.CreateWorld(GameController.database.data.bioms, {GameController.database.data.groups.land, GameController.database.data.groups.land})
   //luanet.GameFacade.world:InitBillboards();
   //luanet.GameFacade.world:InitDecals();  
 }
 public void Scatter()  {
   var world = luanet.GameFacade.world;
   var scattering = world.scattering;
   var scatterTbl = GameController.database.data.scatter;
   // Get biom squares  
   var tmpClassTable = {}
   Console:Message("Scatter objects..");
   for(k, v in pairs(scatterTbl) ){
     Console:Message("Scatter " .. k .. " started..");
     coroutine.yield();
     tmpClassTable[k] = require(GameController.database.data.prefabs[k].require);
     for(biomIdx, biom in pairs(v.biom) ){
       var biomSquare = luanet.GameFacade.world:GetBiomSquare(biom);
       var scatterType = v.biom_scatter_type[biomIdx];
       var count;
       if(scatterType == "stepping"  ){ 
         count = v.biom_count[biomIdx];
       }else{
         count = math.floor(v.biom_count[biomIdx] * biomSquare);
       }
       if(count > 0  ){ 
         var radius = GameController.database.data.prefabs[k].radius  ||  1        ;
         var scatterMethod = "ScatterRandomLua";
         if(scatterType == "random"  ){ 
           scatterMethod = "ScatterRandomLua";
         }else{if scatterType == "billow"  ){ 
           scatterMethod = "ScatterBillowLua";
         }else{if scatterType == "stepping"  ){ 
           scatterMethod = "ScatterSteppingLua";
         }
         scattering[scatterMethod](scattering, biom, count, radius, k, v.fallScore  ||  5);
       }
     }
   }
   coroutine.yield();
   Console:Message("Finalize scattering..");
   coroutine.yield();
   var callCount = 0;
   var callbackFunction = public void (x, y, z, entityName){
     var storage = luanet.Whalebox.WorldStream.Grid.SpaceStorage();
     tmpClassTable[entityName].Create(storage, x, y, z, scatterTbl[entityName]);
     storage:SetString("type", entityName);
     GameController.worldController:RegisterStorage(storage)        ;
     callCount = callCount + 1;
   }
   scattering:Finalize(callbackFunction);
   coroutine.yield();
   Console:Message("Scatter " .. callCount .. " objects");
   coroutine.yield();
 }
 }}