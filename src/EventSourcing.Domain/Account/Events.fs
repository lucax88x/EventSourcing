namespace EventSourcing.Domain.Account

open System
open EventSourcing.Domain.Base

    module Events =
        type AccountCreated(id: Guid, name: string) =
            inherit Event()

            new() = AccountCreated(Guid.Empty, String.Empty)
                
            member val Id = id with get, set
            member val Name = name with get, set

        type NameChanged(name: string) =
            inherit Event()

            new() = NameChanged(String.Empty)

            member val Name = name with get, set