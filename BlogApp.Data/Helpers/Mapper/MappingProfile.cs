using AutoMapper;
using BlogApp.Data.Dto.Article;
using BlogApp.Data.Dto.ArticleVote;
using BlogApp.Data.Dto.Comment;
using BlogApp.Data.Dto.RoleRequest;
using BlogApp.Data.Dto.Roles;
using BlogApp.Data.Dto.User;
using BlogApp.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Data.Helpers.Mapper
{
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Mapper
        /// </summary>
        public MappingProfile()
        {
            //User
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<RegisterDto, User>();
            CreateMap<UserUpdateDto, User>();
            CreateMap<User, UserDetailDto>();

            //Role
            CreateMap<IdentityRole, RoleDto>();

            //RoleRequest
            CreateMap<RoleRequest, RoleRequestListDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name));

            CreateMap<RoleRequest, RoleRequestDetailDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name));

            //Article
            CreateMap<Article, ArticleListDto>();
            CreateMap<Article, ArticleDetailDto>();
            CreateMap<ArticleCreateDto, Article>();
            CreateMap<ArticleUpdateDto, Article>();
            CreateMap<ArticleDetailDto, ArticleUpdateDto>();
            CreateMap<Article, ArticleWithCommentDto>();
            CreateMap<Article, ArticleTagDto>();
            CreateMap<Article, ReportedArticleDto>();
            CreateMap<Article, ArticleDto>();

            //ArticleVote
            CreateMap<ArticleVoteCreateDto, ArticleVote>();
            CreateMap<ArticleVote, ArticleVoteReadDto>();

            //Comment
            CreateMap<Comment, CommentReadDto>();
            CreateMap<CommentCreateDto, Comment>();
            CreateMap<CommentEditDto, Comment>();
            CreateMap<Comment, ReportedCommentDto>();
        }
    }
}
