Все команды выполнить в папке k8s

1. Команда установки БД из helm, вместе с файлом values.yaml.
helm install pg bitnami/postgresql -f postgres/values.yaml

2. Команда применения первоначальных миграций
kubectl apply -f sql-migrations

3. Команда запуска приложения
helm install user-app user-chart