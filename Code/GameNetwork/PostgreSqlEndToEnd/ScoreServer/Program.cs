using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace ScoreServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string? connectionString =
                Environment.GetEnvironmentVariable("GAME_DB_CONNECTION");

            if (String.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "GAME_DB_CONNECTION 환경 변수를 설정한 뒤 실행하세요.");
            }

            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // NpgsqlDataSource는 연결 풀을 관리합니다. 서버가 실행되는 동안 하나만 만듭니다.
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);
            builder.Services.AddSingleton<NpgsqlDataSource>(dataSource);

            WebApplication app = builder.Build();

            app.MapGet("/health", GetHealth);
            app.MapPost("/scores", SaveScoreAsync);

            // 이 참고 예제는 같은 PC에서만 HTTP로 실행합니다.
            app.Run("http://127.0.0.1:5080");
        }

        private static IResult GetHealth()
        {
            return Results.Ok(new { status = "ok" });
        }

        private static async Task<IResult> SaveScoreAsync(
            ScoreRequest request,
            NpgsqlDataSource dataSource)
        {
            if (request.PlayerId <= 0 || request.Score < 0)
            {
                return Results.BadRequest(new
                {
                    error = "PlayerId는 1 이상이고 Score는 0 이상이어야 합니다."
                });
            }

            await using (NpgsqlCommand command = dataSource.CreateCommand(@"
INSERT INTO player_score (player_id, score)
VALUES ($1, $2)
ON CONFLICT (player_id)
DO UPDATE SET
    score = EXCLUDED.score,
    updated_at = CURRENT_TIMESTAMP
RETURNING player_id, score, updated_at;"))
            {
                // 값은 SQL 문자열에 이어 붙이지 않고 매개 변수로 전달합니다.
                command.Parameters.AddWithValue(request.PlayerId);
                command.Parameters.AddWithValue(request.Score);

                await using (NpgsqlDataReader reader =
                             await command.ExecuteReaderAsync())
                {
                    if (!await reader.ReadAsync())
                    {
                        return Results.Problem("점수 저장 결과를 읽지 못했습니다.");
                    }

                    ScoreResponse response = new ScoreResponse();
                    response.PlayerId = reader.GetInt32(0);
                    response.Score = reader.GetInt32(1);
                    response.UpdatedAt = reader.GetDateTime(2);

                    Console.WriteLine(
                        "점수 저장: PlayerId=" + response.PlayerId +
                        ", Score=" + response.Score);

                    return Results.Ok(response);
                }
            }
        }
    }

    public class ScoreRequest
    {
        public int PlayerId { get; set; }
        public int Score { get; set; }
    }

    public class ScoreResponse
    {
        public int PlayerId { get; set; }
        public int Score { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
