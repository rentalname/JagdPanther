﻿using System;
using RedditSharp.Things;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReactiveUI;
using System.Threading.Tasks;
using System.Reactive;

namespace JagdPanther.Model
{
    public class ViewComment
    {
        public ViewComment ThisObject { get { return this; } }
        public int CommentNumber { get; set; }
        public string Body { get; set; }
        public int? ParentAnchor { get; set; }
        public string FlairText { get; set; }
        public string Author { get; set; }
        public string BodyHtml { get; set; }
        public List<ViewComment> Children { get; set; }
        public DateTime Created { get; set; }
        public string Source { get; set; }
        public bool IsGenerator { get { return CommentNumber == 1; } }
        public bool HasBody { get { return (Body != null || Body != String.Empty); } }
        public static explicit operator ViewComment(Comment v)
        {
            var lvc = new List<ViewComment>();
            v.Comments.ToList().ForEach(x =>
                {
                    lvc.Add((ViewComment)x);
                });
            var vc = new ViewComment()
            {
                FlairText = v.AuthorFlairText,
                Body = v.Body,
                Author = v.Author,
                BodyHtml = v.BodyHtml,
                Children = lvc,
                Created = v.Created,
                ParentPost = (Post)v.Parent,
                BaseComment = v
            };
            return vc;
        }

        public string AnchorText
        {
            get
            {
                var num = ParentAnchor;
                if (num.HasValue)
                    return ">>"+num;
                else return null;
            }
        }

        public bool IsExistParentAnchor
        {
            get { return ParentAnchor.HasValue; }
        }
        public Comment BaseComment { get; set; }
        public ViewComment Parent { get; internal set; }
        public Post ParentPost { get; set; }

        public IReactiveCommand<Unit> WriteCommentDialogOpenCommand { get; set; }

        public async Task OpenWriteCommentDialogExcute(object sender)
        {
            var vc = sender as ViewComment;
            if (vc.BaseComment == null)
            {
                var dia = new Dialogs.OpenWriteWindow();

                dia.ShowDialog();
                if (dia.IsClickWriteButton == true)
                    vc.ParentPost.Comment(dia.WriteCommentData);
            }
            else
            {
                var dia = new Dialogs.OpenWriteWindow();

                if (dia.ShowDialog() == true)
                    vc.BaseComment.Reply(dia.WriteCommentData);
            }
        }
        public ViewComment()
        {
            WriteCommentDialogOpenCommand = ReactiveCommand.CreateAsyncTask(OpenWriteCommentDialogExcute);

        }
    }
}