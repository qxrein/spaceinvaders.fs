{
  description = "space invaders flake.nix";

  inputs.nixpkgs.url = "github:NixOS/nixpkgs/nixos-24.05";

  outputs = { self, nixpkgs }: 
    let
      supportedSystems = [ "x86_64-linux" "aarch64-linux" ];
      forAllSystems = f: nixpkgs.lib.genAttrs supportedSystems (system:
        let pkgs = import nixpkgs { inherit system; };
        in f system pkgs
      );
    in
    {
      devShells = forAllSystems (system: pkgs: {
        default = pkgs.mkShell {
          name = "fsharp-shell";
          buildInputs = [
            pkgs.dotnet-sdk_8
            pkgs.jj
          ];
          shellHook = ''
            echo "(dotnet-sdk_8)"
            export DOTNET_CLI_TELEMETRY_OPTOUT=1
          '';
        };
      });
    };
}
