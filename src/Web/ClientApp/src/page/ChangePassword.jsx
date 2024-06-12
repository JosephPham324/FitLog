import { Icon } from "@iconify/react/dist/iconify.js";
// import { emptyAvatar } from "../data/data";

export const ChangePassword = () => {
  return (
    <>
      <div className="bg-neutral-300 pt-20 pb-10 px-10">
        <div className="flex">
          <div className="w-1/4 text-center">
            <Icon className="text-5xl" icon="ic:baseline-arrow-back" />
            <img
              alt="avatar"
              src={
                "https://static.vecteezy.com/system/resources/thumbnails/036/280/651/small/default-avatar-profile-icon-social-media-user-image-gray-avatar-icon-blank-profile-silhouette-illustration-vector.jpg"
              }
              className="rounded-full mb-5 m-auto"
            />
            <div className="font-bold mb-3 text-lg">Nguyen Van A</div>
            <div className="font-bold text-neutral-500 mb-3 tracking-wider">
              nguyenvana@gmail.com
            </div>
            <div className="font-bold mb-5">Change password</div>
          </div>
          <div className="w-3/4 rounded-xl bg-white px-5 pt-10 pb-3">
            <div className="mb-5 text-4xl font-black">CHANGE PASSWORD</div>
            <div className="">
              <div className="mb-4">
                <div className="pl-2 text-neutral-500 mb-1 font-medium text-sm">
                  Current password
                </div>
                <input className="w-1/3 bg-neutral-300 text-neutral-600 py-3 px-2 font-medium rounded-xl" />
              </div>
              <div className="mb-4">
                <div className="pl-2 text-neutral-500 mb-1 font-medium text-sm">
                  New password
                </div>
                <input className="w-1/3 bg-neutral-300 text-neutral-600 py-3 px-2 font-medium rounded-xl" />
              </div>
              <div className="mb-4">
                <div className="pl-2 text-neutral-500 mb-1 font-medium text-sm">
                  Confirm password
                </div>
                <input className="w-1/3 bg-neutral-300 text-neutral-600 py-3 px-2 font-medium rounded-xl" />
              </div>
            </div>

            

            <button className="my-5 px-4 py-3 rounded-lg m-auto bg-green-600 text-white font-medium block hover:bg-green-500">
              Update
            </button>
          </div>
        </div>
      </div>
    </>
  );
};
