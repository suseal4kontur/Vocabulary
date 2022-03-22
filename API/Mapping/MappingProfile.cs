using AutoMapper;
using ViewEntries = View.Entries;
using ViewMeanings = View.Meanings;
using ModelEntries = Model.Entries;
using ModelMeanings = Model.Meanings;
using System;

namespace VocabularyAPI.Mapping
{
    internal sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.AllowNullCollections = true;

            this.CreateMap<ViewMeanings.Meaning, ModelMeanings.Meaning>(MemberList.None)
                .ForMember(m => m.PartOfSpeech, opt => opt.MapFrom(s => 
                    Enum.Parse(typeof(ModelMeanings.PartOfSpeech), s.PartOfSpeech, true)));

            this.CreateMap<ModelMeanings.Meaning, ViewMeanings.Meaning>(MemberList.None)
                .ForMember(m => m.PartOfSpeech, opt => opt.MapFrom(s =>
                    Enum.GetName(typeof(ModelMeanings.PartOfSpeech), s.PartOfSpeech)));

            this.CreateMap<ViewMeanings.MeaningShortInfo, ModelMeanings.Meaning>(MemberList.None)
                .ForMember(m => m.PartOfSpeech, opt => opt.MapFrom(s =>
                    Enum.Parse(typeof(ModelMeanings.PartOfSpeech), s.PartOfSpeech, true)));

            this.CreateMap<ModelMeanings.Meaning, ViewMeanings.MeaningShortInfo>(MemberList.None)
                .ForMember(m => m.PartOfSpeech, opt => opt.MapFrom(s =>
                    Enum.GetName(typeof(ModelMeanings.PartOfSpeech), s.PartOfSpeech)));

            this.CreateMap<ViewMeanings.MeaningCreateInfo, ModelMeanings.MeaningCreateInfo>(MemberList.None).ReverseMap();

            this.CreateMap<ViewMeanings.MeaningUpdateInfo, ModelMeanings.MeaningUpdateInfo>(MemberList.None)
                .ForMember(m => m.PartOfSpeech, opt => opt.MapFrom(s =>
                    Enum.Parse(typeof(ModelMeanings.PartOfSpeech), s.PartOfSpeech, true)));

            this.CreateMap<ModelMeanings.MeaningUpdateInfo, ViewMeanings.MeaningUpdateInfo>(MemberList.None)
                .ForMember(m => m.PartOfSpeech, opt => opt.MapFrom(s =>
                    Enum.GetName(typeof(ModelMeanings.PartOfSpeech), s.PartOfSpeech)));

            this.CreateMap<ViewEntries.EntryShortInfo, ModelEntries.Entry>(MemberList.None).ReverseMap();
            this.CreateMap<ViewEntries.Entry, ModelEntries.Entry>(MemberList.None).ReverseMap();
            this.CreateMap<ViewEntries.EntryCreateInfo, ModelEntries.EntryCreateInfo>(MemberList.None).ReverseMap();
            this.CreateMap<ViewEntries.EntryUpdateInfo, ModelEntries.EntryUpdateInfo>(MemberList.None).ReverseMap();
            this.CreateMap<ViewEntries.EntriesList, ModelEntries.EntriesList>(MemberList.None).ReverseMap();

            this.CreateMap<ViewEntries.EntriesSearchInfo, ModelEntries.EntriesSearchInfo>(MemberList.None)
                .ForMember(m => m.PartOfSpeech, opt => opt.MapFrom(s =>
                    Enum.Parse(typeof(ModelMeanings.PartOfSpeech), s.PartOfSpeech, true)));

            this.CreateMap<ModelEntries.EntriesSearchInfo, ViewEntries.EntriesSearchInfo>(MemberList.None)
                .ForMember(m => m.PartOfSpeech, opt => opt.MapFrom(s =>
                    Enum.GetName(typeof(ModelMeanings.PartOfSpeech), s.PartOfSpeech)));
        }
    }
}
