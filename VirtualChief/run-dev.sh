#!/usr/bin/env bash
#
# Standard startup for the migrated VirtualChief web app.
#
# Targets the local MariaDB/MySQL instance (127.0.0.1:3306) using the merged
# 'virtualchief' database (vcmain schema + tenant tables). Per-workspace tenant
# databases (names from workspaces.name, e.g. kaizenkey/matteo/Testws) must be
# created manually on the same server.
#
# The exports are re-applied from scratch on every start so stale values left
# over in the calling shell can never shadow them.
#
set -euo pipefail

unset VC_VCMAIN_CONN VC_MASTERDB_CONN
export VC_VCMAIN_CONN='Server=127.0.0.1;Port=3306;Uid=matteo;Pwd=hellas;Database=virtualchief'
export VC_MASTERDB_CONN='Server=127.0.0.1;Port=3306;Uid=matteo;Pwd=hellas;database='

cd "$(dirname "$0")"
exec dotnet run
