namespace HAN.Domain.Entities;

public class CourseSchedule
{
    private List<CourseScheduleLine> _courseScheduleLines { get; set; } = [];

    public bool IsValid(List<Evl> allEvls) => 
            IsValidOrder() && 
            IsComplete(allEvls);
    public bool IsValidOrder() => ValidateCourseScheduleLinesOrder();
    
    public bool IsComplete(List<Evl> allEvls) => ValidateCourseScheduleLinesCompleteness(allEvls);

    private bool ValidateCourseScheduleLinesCompleteness(List<Evl> allEvls)
    {
        foreach (var evl in allEvls)
        {
            var lessonMissing = evl.Lessons.Any(lesson => _courseScheduleLines.All(csl => csl.CourseComponent != lesson));
            var examMissing = evl.Exams.Any(exam => _courseScheduleLines.All(csl => csl.CourseComponent != exam));
            
            if (lessonMissing || examMissing)
                return false;
        }

        return true;
    }
    
    public void AddOrUpdateCourseScheduleLine(ICourseComponent courseComponent, int sequenceWeekNumber)
    {
        var existingCourseScheduleLine = _courseScheduleLines.FirstOrDefault(csl => csl.CourseComponent == courseComponent);
        if (existingCourseScheduleLine != null)
        {
            existingCourseScheduleLine.SequenceWeekNumber = sequenceWeekNumber;
        }
        else
        {
            _courseScheduleLines.Add(new CourseScheduleLine
            {
                CourseComponent = courseComponent,
                SequenceWeekNumber = sequenceWeekNumber
            });
        }
    }
    
    private bool ValidateCourseScheduleLinesOrder()
    {
        // Validate the CourseScheduleLines
        
        //                    88                                          88  
        //                    ""                                          88  
        //                                                                88  
        // 8b      db      d8 88 888888888 ,adPPYYba, 8b,dPPYba,  ,adPPYb,88  
        // `8b    d88b    d8' 88      a8P" ""     `Y8 88P'   "Y8 a8"    `Y88  
        //  `8b  d8'`8b  d8'  88   ,d8P'   ,adPPPPP88 88         8b       88  
        //   `8bd8'  `8bd8'   88 ,d8"      88,    ,88 88         "8a,   ,d88  
        //     YP      YP     88 888888888 `"8bbdP"Y8 88          `"8bbdP"Y8  
        //
        //
        //                  Hello I am the validation wizard
        //                  I am here to help you validate
        //                              /\
        //                             /  \
        //                            |    |
        //                          --:'''':--
        //                            :'_' :
        //                            _:"":\___
        //             ' '      ____.' :::     '._
        //            . *=====<<=)           \    :
        //             .  '      '-'-'\_      /'._.'
        //                              \====:_ ""
        //                             .'     \\
        //                            :       :
        //                           /   :    \
        //                          :   .      '.
        //          ,. _            :  : :      :
        //       '-'    ).          :__:-:__.;--'
        //     (        '  )        '-'   '-'
        //  ( -   .00.   - _
        // (    .'  _ )     )
        // '-  ()_.\,\,   -
        
                
        // Each EVL should have the lessons BEFORE the exam. Always\
        // Each CourseSchedule has a CourseScheduleLine for each component that are connected to the EVL's
        // A CourseScheduleLine has a CourseComponent and a SequenceWeekNumber.
        
        // Goal: Make sure that Each Evl has:
        // 1. All EVL's and their lessons are in the schedule
        // 2. The lessons are before the exam
        // 3. The lessons are in the correct order
        
        // Example logic
        return _courseScheduleLines.Count > 0;
    }
}