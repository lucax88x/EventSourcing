namespace EventSourcing.Domain.Account

open System
open EventSourcing.Domain.Base
open Commands

    type Account () =
        inherit AggregateRoot()        

        member val Name = "" with get, set

        override this.Apply(event: Event) = 
           match event with
           | :? Events.AccountCreated as chg ->
                this.Id <- chg.Id
                this.Name <- chg.Name
                event
           | :? Events.NameChanged as chg -> 
                this.Name <- chg.Name
                event
           | _ -> event

        static member Create(id: Guid, name: string) =
            let account = new Account()
            account.ApplyChange(new Events.AccountCreated(id, name)) |> ignore
            account

        member this.SetName(name: string) =
            this.ApplyChange(new Events.NameChanged(name))
