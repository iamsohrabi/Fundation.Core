using Fundation.Abstractions.CQRS.Command;

namespace Fundation.Core.CQRS.Command;

public abstract record TxInternalCommand : InternalCommand, ITxInternalCommand;
