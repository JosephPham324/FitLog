import React from "react";
export const Exercises = [
  {
    Exercise: "Incline Bench Press",
    Sets: 3,
    TopSet: "8 - 12 reps",
  },
  {
    Exercise: "Neck Extension",
    Sets: 3,
    TopSet: "10 - 15 reps",
  },
  {
    Exercise: "Chest Press (Machine)",
    Sets: 3,
    TopSet: "6 - 10 reps",
  },
  {
    Exercise: "Decline Crunch (Weighted)",
    Sets: 3,
    TopSet: "8 - 12 reps",
  },
  {
    Exercise: "JM Press",
    Sets: 4,
    TopSet: "8 - 12 reps",
  },
];

export default function TrainingBoard() {
  return (
    <div className="bg-neutral-300 flex px-20 py-20">
      <div className="w-1/2">
        <div className="mb-3 rounded-xl bg-neutral-400 p-5">
          <div className="flex items-center mb-5">
            <img
              alt="img"
              src="https://cdn-magazine.nutrabay.com/wp-content/uploads/2023/02/strong-bodybuilder-doing-heavy-weight-exercise-back-machine-1-1067x800.jpg"
              className="rounded-xl mr-5 w-2/6"
            />
            <div>
              <div className="mb-5 text-2xl italic font-bold">PPL Program</div>
              <div className="text-sm italic">Week 1 - Day 5</div>
            </div>
          </div>
          <div className="grid grid-cols-3 mb-5 font-medium gap-3">
            <div className="text-center italic">Exercise</div>
            <div className="text-center italic">Sets</div>
            <div className="text-center italic">Top Set</div>
            {Exercises.map((ex) => (
              <>
                <div>{ex.Exercise}</div>
                <div className="text-center">{ex.Sets}</div>
                <div className="text-right pr-10">{ex.TopSet}</div>
              </>
            ))}
          </div>
          <button className="block m-auto px-5 py-2 rounded-xl bg-green-500 font-medium text-xl">
            Continue Week 1 - Day 5
          </button>
        </div>
        <div className="text-center font-medium">{"<"} 1/13 {">"}</div>
      </div>
      <div className="w-1/2 pl-40 pt-10 italic font-medium">
        <div className="mb-10">
          <div className="flex justify-between items-center mb-5">
            <div className="text-2xl font-bold">Workout Tracker</div>
            <div className="font-light text-sm">All Templates (7) {">"}</div>
          </div>
          <button className="w-full py-3 rounded-xl border-2 border-black">
            Start Empty Workout
          </button>
        </div>

        <div>
          <div className="flex justify-between items-center mb-5">
            <div className="text-2xl font-bold">Program Creator</div>
            <div className="font-light text-sm">All Programs (10) {">"}</div>
          </div>
          <button className="w-full py-3 rounded-xl border-2 border-black">
            + Create Program
          </button>
        </div>
      </div>
    </div>
  );
}
