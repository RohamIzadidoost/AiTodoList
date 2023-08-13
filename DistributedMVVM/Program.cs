// See https://aka.ms/new-console-template for more information
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http; 
Console.WriteLine("Hello, World!");
public class SynchronizedCollection<T> : ObservableCollection<T>
{
    
    public  HttpClient httpClient = new HttpClient();
    public HubConnection connection ; 
    public HubConnection ConnInt ;
    public HubConnection ConnSC ;
    public SynchronizedCollection(string url , List<T> ObjList)
    {      
        connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/SR")
                .Build();
        ConnInt  = new HubConnectionBuilder()
            .WithUrl("http://localhost:5000/SRInt")
            .Build();    
        ConnSC = new HubConnectionBuilder()
            .WithUrl("http://localhost:5000/SRHub")
            .Build();  
        ConnSC.StartAsync().Wait() ; 
        connection.StartAsync().Wait() ; 
        ConnInt.StartAsync().Wait() ; 
        foreach(var v in ObjList){
            this.Items.Add(v) ; 
        }
        ConnSC.On("UpdateWithIndexSR" , (int index , T t) => 
        {
            connection.SendAsync("Success") ; 
            base.SetItem(index , t) ; 
        });
        connection.On("UpdateWithIndexSR" , (int index , T t) => 
        {
            connection.SendAsync("Success") ; 
            base.SetItem(index , t) ; 
        });
        ConnInt.On("DeleteWithIndexSR" , (int index) => 
        {
            base.RemoveItem(index) ; 
        });
        connection.On("CreateSignal", (T j) =>
        {
            this.Items.Add(j) ; 
        });
        
        // connection.On("UpdateSignal", (T j) =>
        // {
            
        //     int idx = this.Items.IndexOf(j) ; 
        //     if(idx != -1)
        //     this.Items[idx] = j ; 
        //     else
        //     connection.SendAsync("Error") ; 
        // });
        connection.On("UpdateSignal", (int index , T t) => 
        {
            connection.SendAsync("Success") ; 
            base.SetItem(index , t) ; 
        });
    }
    protected override void SetItem(Int32 index , T item){
        base.SetItem(index, item);
        //connection.SendAsync("UpdateSR" , item as Serverlib.Job).Wait() ; 
        //ConnSC.SendAsync("UpdateWithIndex" , (index , item)) ;
        connection.SendAsync("UpdateSR" , index , item).Wait() ;
    }
    protected override void RemoveItem(Int32 index){
        var item = this[index] ; 
        connection.SendAsync("DeleteJobSR" , item as Serverlib.Job).Wait() ;
        ConnInt.SendAsync("DeleteWithIndex" , index).Wait() ;  
        base.RemoveItem(index);
    }
    protected override void InsertItem(Int32 index , T item){
        base.InsertItem(index , item) ; 
        connection.SendAsync("CreateSR" , item as Serverlib.Job).Wait() ; 
    }

    public List<T> Objs(){
        List<T> Result = new List<T>(); 
        foreach(var v in this.Items){
            Result.Add(v) ; 
        }
        return Result ; 
    }

}