using Dapper;
using MediatR;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TerebiToKiroku.Application;
using TerebiToKiroku.Application.Configuration.Commands;
using TerebiToKiroku.Application.Configuration.Data;

namespace TerebiToKiroku.Infrastructure.Processing.InternalCommands
{
    internal class ProcessInternalCommandsCommandHandler : ICommandHandler<ProcessInternalCommandsCommand, Unit>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public ProcessInternalCommandsCommandHandler(
            ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Unit> Handle(ProcessInternalCommandsCommand command, CancellationToken cancellationToken)
        {
            var connection = this._sqlConnectionFactory.GetOpenConnection();

            const string sql = "SELECT " +
                               "[Command].[Type], " +
                               "[Command].[Data] " +
                               "FROM [app].[InternalCommands] AS [Command] " +
                               "WHERE [Command].[ProcessedDate] IS NULL";
            var commands = await connection.QueryAsync<InternalCommandDto>(sql);

            var internalCommandsList = commands.AsList();

            foreach (var internalCommand in internalCommandsList)
            {
                Type type = Assemblies.Application.GetType(internalCommand.Type);
                dynamic commandToProcess = JsonSerializer.Deserialize(internalCommand.Data, type);

                await CommandsExecutor.Execute(commandToProcess);
            }

            return Unit.Value;
        }

        private class InternalCommandDto
        {
            public required string Type { get; set; }

            public required string Data { get; set; }
        }
    }
}