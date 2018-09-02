namespace EventSourcing.Domain.Account

open EventSourcing.Domain.Base
open System

    module Commands =
        type CreateAccount(id: Guid, name: string) = 
            inherit Command()
            
            member this.Id = id
            member this.Name = name
                    
        type SetAccountName(id: Guid, name: string) = 
            inherit Command()
            
            member this.Id = id
            member this.Name = name