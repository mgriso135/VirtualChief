#!/usr/bin/env bash
#
# Provision the characterization database for VirtualChief.Tests.
#
# Runs a PRIVATE MariaDB instance (as the current user, no root/sudo needed) on
# TCP port 3307, then loads the checked-in schema dumps:
#   kaizenkey.sql  -> database kaizenkey
#   vcmain.sql     -> database vcmain
#   vc_dev.sql     -> database vc_dev
#
# Defaults are overridable via env vars:
#   VC_DB_PORT, VC_DB_DIR, VC_DB_USER, VC_DB_PASS
#
# The tests connect with the same env vars (see tests/VirtualChief.Tests/Support/Db.cs).
#
set -euo pipefail

PORT="${VC_DB_PORT:-3307}"
BASE="${VC_DB_DIR:-/tmp/opencode/mdb2}"
USER="${VC_DB_USER:-vc}"
PASS="${VC_DB_PASS:-vc}"
SOCK="$BASE/run/mysqld.sock"
DATA="$BASE/data"
REPO="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"

mkdir -p "$BASE/run" "$BASE/log"

# 1. Initialise the data directory once.
if [ ! -d "$DATA/mysql" ]; then
  mariadb-install-db --datadir="$DATA" --auth-root-authentication-method=normal \
    --skip-test-db --user="$(id -un)" >/dev/null
fi

# 2. Start the server (idempotent).
if ! mysqladmin --socket="$SOCK" -u root ping >/dev/null 2>&1; then
  setsid /usr/sbin/mariadbd --datadir="$DATA" --socket="$SOCK" --port="$PORT" \
    --bind-address=127.0.0.1 --pid-file="$BASE/run/mysqld.pid" --user="$(id -un)" \
    --log-error="$BASE/log/err.log" >"$BASE/log/stdout.log" 2>&1 </dev/null &
  for _ in $(seq 1 30); do
    mysqladmin --socket="$SOCK" -u root ping >/dev/null 2>&1 && break
    sleep 1
  done
fi

# 3. Create the databases and the test user (root@socket controls the instance).
mysql --socket="$SOCK" -u root <<SQL
CREATE DATABASE IF NOT EXISTS kaizenkey CHARACTER SET utf8mb4;
CREATE DATABASE IF NOT EXISTS vcmain    CHARACTER SET utf8mb4;
CREATE DATABASE IF NOT EXISTS vc_dev    CHARACTER SET utf8mb4;
CREATE USER IF NOT EXISTS '$USER'@'localhost' IDENTIFIED BY '$PASS';
CREATE USER IF NOT EXISTS '$USER'@'127.0.0.1' IDENTIFIED BY '$PASS';
GRANT ALL PRIVILEGES ON kaizenkey.* TO '$USER'@'localhost';
GRANT ALL PRIVILEGES ON vcmain.*    TO '$USER'@'localhost';
GRANT ALL PRIVILEGES ON vc_dev.*    TO '$USER'@'localhost';
GRANT ALL PRIVILEGES ON kaizenkey.* TO '$USER'@'127.0.0.1';
GRANT ALL PRIVILEGES ON vcmain.*    TO '$USER'@'127.0.0.1';
GRANT ALL PRIVILEGES ON vc_dev.*    TO '$USER'@'127.0.0.1';
FLUSH PRIVILEGES;
SQL

# 4. Load the dumps as root (the dumps' views carry DEFINER=root@localhost).
for db in kaizenkey vcmain vc_dev; do
  mysql --socket="$SOCK" -u root "$db" < "$REPO/$db.sql"
done

echo "OK: MariaDB on 127.0.0.1:$PORT, user '$USER'/'$PASS', databases kaizenkey/vcmain/vc_dev loaded."
